using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Comnet;

namespace NGCP.UGV.Core
{
    public partial class UGV
    {
        
        /// <summary>
        /// Represent UnixTime
        /// </summary>
        static DateTime UnixTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        /// <summary>
        /// Struct for Authorization
        /// </summary>
        struct commAuth
        {
            public sbyte AuthorizedNode;
            public sbyte position_rate;
            public sbyte attitude_rate;
            public sbyte vehical_id;
            public byte node_id;
            public sbyte UGVLinkID;
            public sbyte UGVLinkIDjr;
        };

        #region Protonet Callbacks and Sender

        /// <summary>
        /// Callback for Enter
        /// </summary>
        /// <param name="link_id"></param>
        /// <param name="header"></param>
        /// <param name="rx_msg"></param>
        /// <param name="node"></param>
        public void OnEnter(
            sbyte link_id,
            ref Comnet.Header header,
            ref Comnet.Enter rx_msg,
            Comnet.Node node)
        {
            //node.SendEnter();
        }

        /// <summary>
        /// Callback for Ping
        /// </summary>
        /// <param name="link_id"></param>
        /// <param name="header"></param>
        /// <param name="rx_msg"></param>
        /// <param name="node"></param>
        public void OnPing(
            sbyte link_id,
            ref Comnet.Header header,
            ref Comnet.Ping rx_msg,
            Comnet.Node node)
        {
            node.SendPong(header.node_dest_id, GetTimeStamp());
        }

        /// <summary>
        /// Callback for Vehicle Authorization Request
        /// </summary>
        /// <param name="link_id"></param>
        /// <param name="header"></param>
        /// <param name="rx_msg"></param>
        /// <param name="node"></param>
        public void OnVehicleAuthRequest(
           sbyte link_id,
           ref Comnet.Header header,
           ref Comnet.VehicleAuthorizationRequest rx_msg,
           Comnet.Node node)
        {
            double timestamp = (DateTime.Now - UnixTime).TotalMilliseconds;
            byte authorizationServices = 0, grantedServices = 0;

            if (rx_msg.vehicle_type == 100)
            {
                /*0 denotes that the vehicle has no authorized nodes, thus proceed with assignment */
                if (rxCommState.AuthorizedNode == 0)
                {
                    rxCommState.AuthorizedNode = (sbyte)header.node_src_id;
                    authorizationServices = 1;
                    grantedServices = 1; /* Denotes that command is authorized */
                    //reset comm control and reset
                    //CommSteering = 127;
                    //CommWheel = 127;
                    //CommOverride = true;
                }
                /*If the vehicle has been authorized, check if a handover is desired*/
                else
                {
                    if (rxCommState.AuthorizedNode == (sbyte)header.node_src_id &&
                        rx_msg.granted_services == 0 &&
                        rx_msg.authorized_services == 0)
                    {
                        authorizationServices = 0;
                        grantedServices = 0;
                        CommOverride = false;
                        rxCommState.AuthorizedNode = 0;
                    }
                    /*Check that the node requesting the handover is the current authorized node*/
                    else if (rxCommState.AuthorizedNode == (sbyte)header.node_src_id)
                    {
                        /*Check that the field for requesting a handover is not null*/
                        if (rx_msg.granted_services != 0)
                        {
                            /*Set the authorized node as the requested handover node, return that no services are granted*/
                            rxCommState.AuthorizedNode = (sbyte)rx_msg.granted_services;
                            authorizationServices = 1;
                            grantedServices = 2; /* denotes a handover */
                            //reset comm control and reset
                            //CommSteering = 127;
                            //CommWheel = 127;
                            //CommOverride = true;
                        }
                        else
                        {
                            /* If the node already is authorized, and is not requesting handover*/
                            authorizationServices = 1;
                            grantedServices = 0; /* denotes that no services granted */
                        }
                    }
                }
            }
            else if (rx_msg.vehicle_type == 200)
            {
                /*Override key is 200 in case of improper handover */
                if (rx_msg.authorized_services == 0)
                {
                    authorizationServices = 0;
                    grantedServices = 0;
                    CommOverride = false;
                    rxCommState.AuthorizedNode = 0;
                }
                else
                {
                    authorizationServices = 1;
                    grantedServices = 1;
                    rxCommState.AuthorizedNode = (sbyte)header.node_src_id;
                }
            }
            else
            {
                authorizationServices = 0; /* invalid key, node not authorized */
                grantedServices = 0; /* no services granted */
            }

            //send reply
            protonet.SendVehicleAuthorizationReply(header.node_src_id, timestamp, rxCommState.node_id, 2,
                authorizationServices, grantedServices);
            
        }

        /// <summary>
        /// 
        /// </summary>
        bool NewJoyStickInput;

        double LastJoyStick = Double.MinValue;

        /// <summary>
        /// Callback for JoyStick Input
        /// </summary>
        /// <param name="link_id"></param>
        /// <param name="header"></param>
        /// <param name="rx_msg"></param>
        /// <param name="node"></param>
        public void OnJoyStickInput(
            sbyte link_id,
            ref Comnet.Header header,
            ref Comnet.VehicleJoystickCommand rx_msg,
            Comnet.Node node)
        {
            //check if input if from authorized node
            if ((sbyte)header.node_src_id == rxCommState.AuthorizedNode)
            {
                if (rx_msg.timestamp > LastJoyStick)
                {
                    LastJoyStick = rx_msg.timestamp;
                    //override just in case
                    CommOverride = true;
                    //scale for -100 to 100
                    //get throttle
                    CommWheel = (byte)(rx_msg.throttle / 256.0f);
                    //get steering
                    CommSteering = (byte)(rx_msg.steering / 256.0f);
                    //set flag
                    NewJoyStickInput = true;

                    DebugMessage.Clear();
                    DebugMessage.Append("Thr = " + rx_msg.throttle + "/" + CommWheel + " Steer = " + rx_msg.steering + "/" + CommSteering);
                }
            }
        }

        /// <summary>
        /// Callback for waypoint command
        /// </summary>
        /*
        public void OnWaypointCommand(
            sbyte link_id,
            ref Comnet.Header header,
            ref Comnet.VehicleWaypointCommand rx_msg,
            Comnet.Node node)
        { 
            
            //check if input if from authorized node
            if ((sbyte)header.node_src_id == rxCommState.AuthorizedNode)
            {
                if (rx_msg.waypoint_type == 0)
                {
                    //add waypoint
                    this.InsertWaypointAt(rx_msg.waypoint_ID, new Navigation.WayPoint(
                        rx_msg.latitude / 10000000.0, rx_msg.longitude / 10000000.0, rx_msg.altitude / 10000000.0));
                }
                else if (rx_msg.waypoint_type == 1)
                {
                   this.EditWaypointAt(rx_msg.waypoint_ID, new Navigation.WayPoint(
                        rx_msg.latitude / 10000000.0, rx_msg.longitude / 10000000.0, rx_msg.altitude / 10000000.0));
                }
                else if (rx_msg.waypoint_type == 2)
                {
                    this.RemoveWaypointAt(rx_msg.waypoint_ID);
                }
            }
        }
        */
        /// <summary>
        /// Receive ugv battery status
        /// </summary>
        /// <param name="linkid"></param>
        /// <param name="header"></param>
        /// <param name="rx_msg"></param>
        /// <param name="node"></param>
        
        /*private void OnUGVBatteryStatus(sbyte linkid, ref Header header, ref UGVBatteryStatus rx_msg, Node node)
        {
            //copy battery status
            BatteryStatus battery = new BatteryStatus();
            battery.Voltage3_3V = rx_msg.voltage_3_3V;
            battery.Voltage5V = rx_msg.voltage_5V;
            battery.Voltage12V = rx_msg.voltage_12V;
            UpdateBattery(battery);
            /*
            string str = "Voltage 3.3: " + rx_msg.voltage_3_3V;
            str = "Voltage 5: " + rx_msg.voltage_5V;
            str = "Voltage 12: " + rx_msg.voltage_12V;
            DebugMessage.Clear();
            DebugMessage.Append(str);*/
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="linkid"></param>
        /// <param name="header"></param>
        /// <param name="rx_msg"></param>
        /// <param name="node"></param>
        private void OnPayloadBayCommand(sbyte linkid, ref Header header, ref PayloadBayCommand rx_msg, Node node)
        {
            if ((sbyte)header.node_src_id == rxCommState.AuthorizedNode)
            {
                //if message is for me
                if (header.node_dest_id == this.Settings.UGVProtonetNodeID)
                {
                    if (rx_msg.payload_bay_ID == 1 && rx_msg.payload_command == 1)
                        TargetDropped = true;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="linkid"></param>
        /// <param name="header"></param>
        /// <param name="rx_msg"></param>
        /// <param name="node"></param>
        private void OnVehicleModeCommand(sbyte linkid, ref Header header, ref VehicleModeCommand rx_msg, Node node)
        {
            if ((sbyte)header.node_src_id == rxCommState.AuthorizedNode)
            {
                //if message is for me
                if (header.node_dest_id == this.Settings.UGVProtonetNodeID)
                {
                    //0xAB
                    //A = 0 Reset All
                    //  B = NC
                    //A = 1 DriveMode Command
                    //  B = 0 Local
                    //  B = 1 Semi
                    //  B = 2 Auto
                    //A = 2 Autonoumous State
                    //  B = 0 Search
                    //A = F Enable Switch
                    //  B = 0 Enable
                    //  B = 7 Toggle
                    //  B = F Disabe
                    byte A = (byte)((int)rx_msg.vehicle_mode >> 4);
                    byte B = (byte)((int)rx_msg.vehicle_mode & 0x0F);
                    if (A == 0)
                    {
                        this.LocalSpeed = 0;
                        this.LocalSteering = 0;
                        this.Settings.DriveMode = DriveMode.LocalControl;
                        this.ResetAllState();
                    }
                    else if (A == 1)
                    {
                        if (B == 0)
                            this.Settings.DriveMode = DriveMode.LocalControl;
                        else if (B == 1)
                            this.Settings.DriveMode = DriveMode.SemiAutonomous;
                        else if (B == 2)
                            this.Settings.DriveMode = DriveMode.Autonomous;
                    }
                    else if (A == 2)
                    {
                        if (B == 0)
                            this.State = DriveState.SearchTarget;
                    }
                    else if (A == 15)
                    {
                        if (B == 0)
                            this.Enabled = true;
                        else if (B == 7)
                            this.Enabled = !this.Enabled;
                        else if (B == 15)
                            this.Enabled = false;
                    }
                }
            }
        }

        /// <summary>
        /// Send a state to GCS
        /// </summary>
        /// <param name="state"></param>
        public void SendState(UGVState state)
        {
            int sAltitude = 68;
            if (rxCommState.AuthorizedNode > 0)
            {
                double timestamp = GetTimeStamp();
                protonet.SendVehicleGlobalPosition((byte)Settings.CommNode, timestamp, (ushort)rxCommState.UGVLinkID,
                    (int)(state.Latitude * 10000000.0), (int)(state.Longitude * 10000000.0), 1,  0, 0, 0);
                protonet.SendVehicleAttitude((byte)Settings.CommNode, timestamp, rxCommState.node_id,
                    (float)state.Roll, (float)state.Pitch, 0);
                if (state.TargetLocation != null)
                {
                    //protonet.SendTargetDesignationCommand((byte)Settings.CommNode, timestamp, (ushort)rxCommState.UGVLinkID,
                    //    1, 1, 0,
                    //    (int)(state.TargetLocation.Lat * 10000000.0), (int)(state.TargetLocation.Long * 10000000.0), sAltitude);
                }
            }
        }

        #endregion Protonet Callbacks and Senders

        /// <summary>
        /// Generate the time stamp
        /// </summary>
        /// <returns></returns>
        public static double GetTimeStamp()
        {
            return (DateTime.Now - UnixTime).TotalMilliseconds;
        }

        #region Protonet Methods

        /// <summary>
        /// Start Protonet
        /// </summary>
        public void startProtonet()
        {
            rxCommState.attitude_rate = Settings.AttiudeRate;
            rxCommState.position_rate = Settings.PositionRate;
            rxCommState.node_id = Settings.UGVProtonetNodeID;

            protonet = new Comnet.Node(rxCommState.node_id);

            protonet.RegisterEnterEvent(this.OnEnter);
            protonet.RegisterPingEvent(this.OnPing);
            protonet.RegisterVehicleAuthorizationRequestEvent(this.OnVehicleAuthRequest);
            protonet.RegisterVehicleJoystickCommandEvent(this.OnJoyStickInput);
            //protonet.RegisterVehicleWaypointCommandEvent(this.OnWaypointCommand);
            //protonet.RegisterUGVBatteryStatusEvent(this.OnUGVBatteryStatus);
            protonet.RegisterPayloadBayCommandEvent(this.OnPayloadBayCommand);
            protonet.RegisterVehicleModeCommandEvent(this.OnVehicleModeCommand);
            protonet.AddZigBeeDatalink(out rxCommState.UGVLinkID, (ushort)Settings.CommBaud, Settings.CommPort);
            //protonet.EstablishZigBeeEndpoint(rxCommState.UGVLinkID, 4, "0013A2004067BF33");
            protonet.EstablishZigBeeEndpoint(rxCommState.UGVLinkID, 1, "0013A2004091798F");
            
            //protonet.AddUDPDatalink(out rxCommState.UGVLinkIDjr, 81);

            //timer
            System.Timers.Timer JoyStickTimer = new System.Timers.Timer(2000);
            JoyStickTimer.Elapsed += JoyStickTimer_Tick;
            JoyStickTimer.Start();
        }

        /// <summary>
        /// On joystick timer tick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void JoyStickTimer_Tick(object sender, System.Timers.ElapsedEventArgs args)
        {
            if (rxCommState.AuthorizedNode > 0)
            {
                //set false
                NewJoyStickInput = false;
                //wait 2 sec
                System.Threading.Thread.Sleep(1000);
                protonet.SendPing((byte)rxCommState.UGVLinkID, 0.3f);
                //check if number got set
                if (!NewJoyStickInput)
                {
                    //get off override mode
                    CommOverride = false;
                }
            }
        }

        

        #endregion Protonet Methods
    }
}
