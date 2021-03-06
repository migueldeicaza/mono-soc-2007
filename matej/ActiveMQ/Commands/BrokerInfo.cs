/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
//
//  NOTE!: This file is autogenerated - do not modify!
//         if you need to make a change, please see the Groovy scripts in the
//         activemq-core module
//

using System;
using System.Collections;

using ActiveMQ.OpenWire;
using ActiveMQ.Commands;

namespace ActiveMQ.Commands
{
    /// <summary>
    ///  The ActiveMQ BrokerInfo Command
    /// </summary>
    public class BrokerInfo : BaseCommand
    {
        public const byte ID_BrokerInfo = 2;
    			
        BrokerId brokerId;
        string brokerURL;
        BrokerInfo[] peerBrokerInfos;
        string brokerName;
        bool slaveBroker;
        bool masterBroker;
        bool faultTolerantConfiguration;
        bool duplexConnection;
        bool networkConnection;
        long connectionId;

		public override string ToString() {
            return GetType().Name + "["
                + " BrokerId=" + BrokerId
                + " BrokerURL=" + BrokerURL
                + " PeerBrokerInfos=" + PeerBrokerInfos
                + " BrokerName=" + BrokerName
                + " SlaveBroker=" + SlaveBroker
                + " MasterBroker=" + MasterBroker
                + " FaultTolerantConfiguration=" + FaultTolerantConfiguration
                + " DuplexConnection=" + DuplexConnection
                + " NetworkConnection=" + NetworkConnection
                + " ConnectionId=" + ConnectionId
                + " ]";

		}

        public override byte GetDataStructureType() {
            return ID_BrokerInfo;
        }


        // Properties

        public BrokerId BrokerId
        {
            get { return brokerId; }
            set { this.brokerId = value; }            
        }

        public string BrokerURL
        {
            get { return brokerURL; }
            set { this.brokerURL = value; }            
        }

        public BrokerInfo[] PeerBrokerInfos
        {
            get { return peerBrokerInfos; }
            set { this.peerBrokerInfos = value; }            
        }

        public string BrokerName
        {
            get { return brokerName; }
            set { this.brokerName = value; }            
        }

        public bool SlaveBroker
        {
            get { return slaveBroker; }
            set { this.slaveBroker = value; }            
        }

        public bool MasterBroker
        {
            get { return masterBroker; }
            set { this.masterBroker = value; }            
        }

        public bool FaultTolerantConfiguration
        {
            get { return faultTolerantConfiguration; }
            set { this.faultTolerantConfiguration = value; }            
        }

        public bool DuplexConnection
        {
            get { return duplexConnection; }
            set { this.duplexConnection = value; }            
        }

        public bool NetworkConnection
        {
            get { return networkConnection; }
            set { this.networkConnection = value; }            
        }

        public long ConnectionId
        {
            get { return connectionId; }
            set { this.connectionId = value; }            
        }

    }
}
