/*
 * Copyright (c) 2012 Alexander Houben (ahouben@greenliff.com)
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of
 * this software and associated documentation files (the "Software"), to deal in
 * the Software without restriction, including without limitation the rights to
 * use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
 * of the Software, and to permit persons to whom the Software is furnished to do
 * so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */
/*
 * Modifications:
 * 2014 Jorge Garza (electronica2@fabatsa.com.mx)
 */

using System;
using System.IO.Ports;
using System.IO;
using System.Threading;
using System.Collections.Generic;

using System.Linq;

namespace BgApiDriver
{
    public partial class BgApi
    {
        // Set this to true to show BLE Driver Logs in Console.
        public const bool DEBUG_BLE = false;

        /// <summary>
        /// The serial port we use to communicate with the BLE dongle.
        /// </summary>
        private SerialPort m_serialPort;

        /// <summary>
        /// The port name that the BLE dongle is connected to, ex. "COM3".
        /// </summary>
        private string m_port;

        /// <summary>
        /// The stream connected to <code>m_serialPort</code> we use to read/write to/from the BLE dongle.
        /// </summary>
        private Stream m_stream;

        /// <summary>
        /// Indicates whether the underlying serial port is open.
        /// </summary>
        public bool IsOpen { get { return m_serialPort != null && m_serialPort.IsOpen; } }

        /// <summary>
        /// Information about the ble device.
        /// </summary>
        public ble_msg_system_get_info_rsp_t Info { get; private set; }

        /// <summary>
        /// Wait handle for a response from the ble device.
        /// </summary>
        protected ManualResetEvent m_waitHandleResponse;

        /// <summary>
        /// Holds the response of a previously sent bgapi BgApiCommand.
        /// </summary>
        private BgApiResponse m_response;

        /// <summary>
        /// The event handler called when new data is received at the serial port.
        /// </summary>
        private SerialDataReceivedEventHandler m_serialDataReceivedEventHandler;

        /// <summary>
        /// Assumes that the maximum message size in bytes that ever goes over the wire in both directions is less than this value.
        /// </summary>
        public const int MAX_MESSAGE = 256;

        /// <summary>
        /// The maximum receive message size in bytes.
        /// </summary>
        public const int MAX_RECEIVE_MESSAGE = MAX_MESSAGE;

        /// <summary>
        /// The size of the header in bytes.
        /// </summary>
        public const int SIZE_HEADER = 4;

        /// <summary>
        /// Default wait time for the arrival of an event.
        /// </summary>
        public const int EVENT_TIMEOUT_DEFAULT = 1000;

        /// <summary>
        /// Indicates id conenction is been used by another application
        /// </summary>
        public bool CONN_BUSY = false;

        /// <summary>
        /// Receive buffer.
        /// </summary>
        byte[] m_rx;

        /// <summary>
        /// The number of valid receive bytes in the receive buffer <code>m_rx</code>.
        /// </summary>
        int m_rxOffset;

        /// <summary>
        /// Contains a list of scanned bluetooth low energy devices 
        /// </summary>
        public List<ble_msg_gap_scan_response_evt_t> deviceList = new List<ble_msg_gap_scan_response_evt_t>();

        /// <summary>
        /// Maps errors to strings.
        /// </summary>
        private Dictionary<ble_error, string> m_ble_error2string = new Dictionary<ble_error,string>()
        {
            { ble_error.ble_err_bt_error_success, "Command completed successfully" },
            { ble_error.ble_err_bt_bt_error_authentication_failure, "Pairing or authentication failed due to incorrect results in the pairing or authentication procedure. This could be due to an incorrect PIN or Link Key." },
            { ble_error.ble_err_bt_pin_or_key_missing, "Pairing failed because of missing PIN, or authentication failed because of missing Key." },
            { ble_error.ble_err_bt_bt_error_memory_capacity_exceeded, "Controller is out of memory."},
            { ble_error.ble_err_bt_connection_timeout, "Link supervision timeout has expired." },
            { ble_error.ble_err_bt_connection_limit_exceeded, "Controller is at limit of connections it can support." },
            { ble_error.ble_err_bt_bt_error_command_disallowed, "Command requested cannot be executed because the Controller is in a state where it cannot process this command at this time." },
            { ble_error.ble_err_bt_bt_error_invalid_command_parameters, "Command contained invalid parameters." },
            { ble_error.ble_err_bt_bt_error_remote_user_terminated, "User on the remote device terminated the connection." },
            { ble_error.ble_err_bt_bt_error_connection_terminated_by_local_host, "Local device terminated the connection." },
            { ble_error.ble_err_bt_bt_error_ll_response_timeout, "Connection terminated due to link-layer procedure timeout." },
            { ble_error.ble_err_bt_bt_error_ll_instant_passed, "Received link-layer control packet where instant was in the past." },
            { ble_error.ble_err_bt_bt_error_controller_busy, "Operation was rejected because the controller is busy and unable to process the request." },
            { ble_error.ble_err_bt_bt_error_directed_advertising_timeout, "Directed advertising completed without a connection being created." },
            { ble_error.ble_err_bt_bt_error_connection_terminated_due_to_mic_failure, "Connection was terminated because the Message Integrity Check (MIC) failed on a received packet." },
            { ble_error.ble_err_bt_bt_error_connection_failed_to_be_established, "LL initiated a connection but the connection has failed to be established. Controller did not receive any packets from remote end." },
            { ble_error.ble_err_att_invalid_handle, "The attribute handle given was not valid on this server" },
            { ble_error.ble_err_att_read_not_permitted, "The attribute cannot be read" },
            { ble_error.ble_err_att_write_not_permitted, "The attribute cannot be written" },
            { ble_error.ble_err_att_invalid_pdu, "The attribute PDU was invalid" },
            { ble_error.ble_err_att_insufficient_authentication, "The attribute requires authentication before it can be read or written." },
            { ble_error.ble_err_att_request_not_supported, "Attribute Server does not support the request received from the client." },
            { ble_error.ble_err_att_invalid_offset, "Offset specified was past the end of the attribute" },
            { ble_error.ble_err_att_insufficient_authorization, "The attribute requires authorization before it can be read or written." },
            { ble_error.ble_err_att_prepare_queue_full, "Too many prepare writes have been queued" },
            { ble_error.ble_err_att_att_not_found, "No attribute found within the given attribute handle range." },
            { ble_error.ble_err_att_att_not_long, "The attribute cannot be read or written using the Read Blob Request" },
            { ble_error.ble_err_att_insufficient_enc_key_size, "The Encryption Key Size used for encrypting this link is insufficient." },
            { ble_error.ble_err_att_invalid_att_length, "The attribute value length is invalid for the operation" },
            { ble_error.ble_err_att_unlikely_error, "The attribute request that was requested has encountered an error that was unlikely, and therefore could not be completed as requested." },
            { ble_error.ble_err_att_insufficient_encryption, "The attribute requires encryption before it can be read or written." },
            { ble_error.ble_err_att_unsupported_group_type, "The attribute type is not a supported grouping attribute as defined by a higher layer specification." },
            { ble_error.ble_err_att_insufficient_resources, "Insufficient Resources to complete the request" },
            { ble_error.ble_err_att_application, "Application error code defined by a higher layer specification." },
            { ble_error.ble_err_success, "No error" },
            { ble_error.ble_err_invalid_param, "Command contained invalid parameter" },
            { ble_error.ble_err_wrong_state, "Device is in wrong state to receive command" },
            { ble_error.ble_err_out_of_memory, "Device has run out of memory" },
            { ble_error.ble_err_not_implemented, "Feature is not implemented" },
            { ble_error.ble_err_invalid_command, "Command was not recognized" },
            { ble_error.ble_err_timeout, "Command or Procedure failed due to timeout" },
            { ble_error.ble_err_not_connected, "Connection handle passed is to command is not a valid handle" },
            { ble_error.ble_err_flow, "Command would cause either underflow or overflow error" },
            { ble_error.ble_err_user_attribute, "User attribute was accessed through API which is not supported" },
            { ble_error.ble_err_smp_passkey_entry_failed, "The user input of passkey failed, for example, the user cancelled the operation" },
            { ble_error.ble_err_smp_oob_not_available, "Out of Band data is not available for authentication" },
            { ble_error.ble_err_smp_authentication_requirements, "The pairing procedure cannot be performed as authentication requirements cannot be met due to IO capabilities of one or both devices" },
            { ble_error.ble_err_smp_confirm_value_failed, "The confirm value does not match the calculated compare value" },
            { ble_error.ble_err_smp_pairing_not_supported, "Pairing is not supported by the device" },
            { ble_error.ble_err_smp_encryption_key_size, "The resultant encryption key size is insufficient for the security requirements of this device" },
            { ble_error.ble_err_smp_command_not_supported, "The SMP command received is not supported on this device" },
            { ble_error.ble_err_smp_unspecified_reason, "Pairing failed due to an unspecified reason" },
            { ble_error.ble_err_smp_repeated_attempts, "Pairing or authentication procedure is disallowed because too little time has elapsed since last pairing request or security request" },
            { ble_error.ble_err_smp_invalid_parameters, "The Invalid Parameters error code indicates: the command length is invalid or a parameter is outside of the specified range." },
            //ble_error.ble_err_last
        };

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="port">ex. "COM23"</param>
        public BgApi(string port)
        {
            m_port = port;
            
            m_rx = new byte[MAX_RECEIVE_MESSAGE];
            m_waitHandleResponse = new ManualResetEvent(false);
        }

        /// <summary>
        /// Opens the connection to a bgapi device.
        /// </summary>
        public virtual bool Open()
        {
            if (!IsOpen)
            {
                /*
                if (!doOpen()) return false;
                // start with a well known device state
                ble_cmd_system_reset(0);
                Close();
                 */
                if (!doOpen()) return false;

                Info = ble_cmd_system_get_info();
                if (Info != null)
                {
                    log(string.Format("Build: {0}, protocol version: {1}, hardware: {2}", Info.build, Info.protocol_version,
                        Info.hw == 0x01 ? "BLE112" : Info.hw == 0x02 | Info.hw == 0x03 ? "BLED112" : string.Format("Unknown({0})", Info.hw)));
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private bool doOpen()
        {
            m_serialPort = new SerialPort(m_port, 256000, Parity.None, 8, StopBits.One);
            // register for data received events
            m_serialDataReceivedEventHandler = new SerialDataReceivedEventHandler(m_serialPort_DataReceived);
            m_serialPort.DataReceived += m_serialDataReceivedEventHandler;

            bool deviceFound = false;
            CONN_BUSY = false;
            for (DateTime now = DateTime.Now; DateTime.Now - now < new TimeSpan(0, 0, 2); )
            {
                try
                {
                    m_serialPort.Open();
                    deviceFound = true;
                    break;
                }
                catch (IOException)
                {
                    Thread.Sleep(100);
                }
                catch (UnauthorizedAccessException)
                {
                    CONN_BUSY = true;     //Device is been used
                    break;
                }
            }
            if (!deviceFound)
            {
                return false;
            }
            m_stream = m_serialPort.BaseStream;
            m_rxOffset = 0;

            return true;
        }

        /// <summary>
        /// Closes the stream and serial port to a BLE device.
        /// </summary>
        public virtual void Close()
        {
            if (m_serialPort != null)
            {
                m_serialPort.DataReceived -= m_serialDataReceivedEventHandler;
                try { m_serialPort.Close(); } catch { }
                m_serialPort = null;
                m_stream = null;
            }
        }

        /*
         *  BLUETOOTH DATA EVETN HANDLE
         */
        protected virtual void HandleEvent(BgApiEvent evt)
        {
            switch (evt.Class)
            {
                case (byte)ble_classes.ble_cls_connection:
                    switch (evt.Id)
                    {
                        case (byte)ble_event_ids.ble_evt_connection_status_id:
                            {
                                ble_msg_connection_status_evt_t msg = new ble_msg_connection_status_evt_t();
                                msg = (ble_msg_connection_status_evt_t)evt;
                                bluetoothLeConnected(msg);
                            }
                            break;

                        case (byte)ble_event_ids.ble_evt_connection_disconnected_id:
                            {
                                ble_msg_connection_disconnected_evt_t msg = new ble_msg_connection_disconnected_evt_t();
                                msg = (ble_msg_connection_disconnected_evt_t)evt;
                                bluetoothLeDisconnected(msg);
                            }
                            break;

                        default:
                            break;
                    }
                    break;

                case (byte)ble_classes.ble_cls_gap:
                    switch (evt.Id)
                    {
                        case (byte)ble_event_ids.ble_evt_gap_scan_response_id:
                            {
                                bool itemInList = false;
                                ble_msg_gap_scan_response_evt_t msg = new ble_msg_gap_scan_response_evt_t();
                                msg = (ble_msg_gap_scan_response_evt_t)evt;

                                if (msg.packet_type == BgApi.GAP_SCAN_HEADER_SCAN_RSP)
                                {
                                    /*
                                    log("NAME= " + System.Text.Encoding.UTF8.GetString(msg.data)
                                       + " RSSI = " + (sbyte)msg.rssi
                                       + " ADDRESS = " + BitConverter.ToString(msg.sender.Address).Replace("-", ":"));
                                     */

                                    //Add the device to the dynamic array if not listed  
                                    for (int i = 0; i < deviceList.Count ; i++)
                                    {
                                        if (deviceList[i].sender.Address.SequenceEqual(msg.sender.Address))
                                        {                                         
                                            deviceList[i] = msg;
                                            itemInList = true;
                                            break;
                                        }
                                    }

                                    if (!itemInList) deviceList.Add(msg);
                                }
                            }
                            break;

                        default:
                            break;
                    }
                    break;

                case (byte)ble_classes.ble_cls_attclient:
                    switch (evt.Id)
                    {
                        case (byte)ble_event_ids.ble_evt_attclient_attribute_value_id:
                            {
                                ble_msg_attclient_attribute_value_evt_t msg = new ble_msg_attclient_attribute_value_evt_t();
                                msg = (ble_msg_attclient_attribute_value_evt_t)evt;
                                bluetoothLeReceivedData(msg);
                               
                                Console.WriteLine("Connection = " + msg.connection +
                                    " atthandle = " + msg.atthandle +
                                    " type = " + msg.type +
                                    " value = " + System.Text.Encoding.ASCII.GetString(msg.value));

                                
                                // Release Semaphore and Stop Timer
                                releaseBLE();                               

                            }
                            break;
                        case (byte)ble_event_ids.ble_evt_attclient_procedure_completed_id:
                            {
                            }
                            break;
                        default:
                            break;
                    }
                    break;

                default:
                    log("There was another message in Class");
                    break;
            }      
        }

        public virtual void bluetoothLeConnected (ble_msg_connection_status_evt_t deviceInfo)
        {
            //Override this function
        }

        public virtual void bluetoothLeDisconnected(ble_msg_connection_disconnected_evt_t deviceInfo)
        {
            //Override this function
        }

        public virtual void bluetoothLeReceivedData(ble_msg_attclient_attribute_value_evt_t deviceInfo)
        {
            //Override this function
        }

        public virtual void bluetoothLeTimeExceed ()
        {
            //Override this function
        }

        private void receive(SerialDataReceivedEventArgs e)
        {
            // log(string.Format("Received: {0}", m_serialPort.BytesToRead));
            int read = m_stream.Read(m_rx, m_rxOffset, m_serialPort.BytesToRead);
            m_rxOffset += read;

            while (true)
            {
                // log(string.Format("m_rxOffset: {0}", m_rxOffset));
                if (m_rxOffset < SIZE_HEADER)
                {
                    // wait for more data
                    //log(string.Format("Waiting for header: {0}", m_rxOffset));
                    return;
                }

                // read payload
                int length = ((m_rx[0] & 0x7F) << 8) | m_rx[1];
                //log(string.Format("length: {0}", length));
                if (m_rxOffset < SIZE_HEADER + length)
                {
                    // wait for more data
                    log(string.Format("Waiting for more data, expected {1}, got {0}", SIZE_HEADER + length, m_rxOffset));
                    return;
                }

                // full msg in m_rx, evt or rsp ?
                byte[] evtRspBuffer = new byte[SIZE_HEADER + length];
                Array.Copy(m_rx, evtRspBuffer, evtRspBuffer.Length);
                // remove first event
                int excessBytes = m_rxOffset - evtRspBuffer.Length;
                for (int i = 0; i < excessBytes; i++)
                {
                    m_rx[i] = m_rx[evtRspBuffer.Length + i];
                }
                m_rxOffset -= evtRspBuffer.Length;
                //log(string.Format("m_rxOffset to {0}", m_rxOffset));

                BgApiEventResponse evtRsp = parseEventResponse(new BgApiEventResponse() { Data = evtRspBuffer });
                if(evtRsp.IsEvent)
                {
                    HandleEvent((BgApiEvent)evtRsp);
                }
                else
                {
                    m_response = (BgApiResponse)evtRsp;
                    m_waitHandleResponse.Set();
                    
                }
            }
        }

        /// <summary>
        /// Event handler for receiving data from the serial stream.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                receive(e);              
            }
            catch (Exception ex)
            {
                log(ex.ToString());
            }
        }

        /// <summary>
        /// Assert helper.
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        private void check(int actual, int expected)
        {
            if (actual != expected)
            {
                throw new BgApiException(string.Format("Length mismatch. Actual {0}, expected {1}", actual, expected));
            }
        }

        /// <summary>
        /// Sends a command and receives a response from a ble device.
        /// </summary>
        /// <param name="command">The BgApiCommand to send.</param>
        /// <param name="no_return">True, iff the command has no response (ex. the reset command does not return a response).</param>
        /// <returns>The response from the bgapi device.</returns>
        private BgApiResponse Send(BgApiCommand command, bool no_return)
        {
            try
            {
                // ensure an open connection before attempting to send
                Open();
                // wait for response
                m_waitHandleResponse.Reset();
                m_response = null;

                //log("--> " + string.Join(" ", command.Data.Select(x => x.ToString("X2"))));

                // write command
                m_stream.Write(command.Data, 0, command.Data.Length);
                m_stream.Flush();

                if (no_return)
                {
                    // do not expect a response for this command
                    return null;
                }

                // what is the maximum wait time for a response
                if (!m_waitHandleResponse.WaitOne(5000))
                {
                    throw new BgApiException("Response timeout");
                }
                //log("<-- " + string.Join(" ", m_response.Data.Select(x => x.ToString("X2"))));
                return m_response;
            }
            catch (Exception e)
            {
                log(e.Message);
                // do not assume anything about the state of the ble device
                // after an exception
                Close();
                return null;
            }
        }

        /// <summary>
        /// All logging goes through this method for easy overriding.
        /// </summary>
        /// <param name="msg">The message to log.</param>
        protected virtual void log(string msg)
        {
            if (DEBUG_BLE)
            {
                Console.WriteLine(msg);
                System.Diagnostics.Debug.Print(msg);
            }
        }
    }
}
