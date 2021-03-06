/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

//
// Marshalling code for Open Wire Format for BaseCommand
//
//
// NOTE!: This file is autogenerated - do not modify!
//        if you need to make a change, please see the Groovy scripts in the
//        activemq-openwire module
//

using ActiveMQ.OpenWire;
using System;



namespace ActiveMQ.Commands
{
    public abstract class BaseCommand : BaseDataStructure, Command
    {
        private int commandId;
        
        public int CommandId
        {
            get { return commandId; }
            set { this.commandId = value; }
        }
        
        public override int GetHashCode()
        {
            return (CommandId * 37) + GetDataStructureType();
        }
        
        public override bool Equals(Object that)
        {
            if (that is BaseCommand)
            {
                BaseCommand thatCommand = (BaseCommand) that;
                return this.GetDataStructureType() == thatCommand.GetDataStructureType()
                    && this.CommandId == thatCommand.CommandId;
            }
            return false;
        }
        
        public override String ToString()
        {
            string answer = GetDataStructureTypeAsString(GetDataStructureType());
            if (answer.Length == 0)
            {
                answer = base.ToString();
            }
            return answer + ": id = " + CommandId;
        }
        
        public static String GetDataStructureTypeAsString(int type)
        {
            String packetTypeStr = "";
            switch (type)
            {
                case ActiveMQMessage.ID_ActiveMQMessage :
                    packetTypeStr = "ACTIVEMQ_MESSAGE";
                    break;
                case ActiveMQTextMessage.ID_ActiveMQTextMessage :
                    packetTypeStr = "ACTIVEMQ_TEXT_MESSAGE";
                    break;
                case ActiveMQObjectMessage.ID_ActiveMQObjectMessage:
                    packetTypeStr = "ACTIVEMQ_OBJECT_MESSAGE";
                    break;
                case ActiveMQBytesMessage.ID_ActiveMQBytesMessage :
                    packetTypeStr = "ACTIVEMQ_BYTES_MESSAGE";
                    break;
                case ActiveMQStreamMessage.ID_ActiveMQStreamMessage :
                    packetTypeStr = "ACTIVEMQ_STREAM_MESSAGE";
                    break;
                case ActiveMQMapMessage.ID_ActiveMQMapMessage :
                    packetTypeStr = "ACTIVEMQ_MAP_MESSAGE";
                    break;
                case MessageAck.ID_MessageAck :
                    packetTypeStr = "ACTIVEMQ_MSG_ACK";
                    break;
                case Response.ID_Response :
                    packetTypeStr = "RESPONSE";
                    break;
                case ConsumerInfo.ID_ConsumerInfo :
                    packetTypeStr = "CONSUMER_INFO";
                    break;
                case ProducerInfo.ID_ProducerInfo :
                    packetTypeStr = "PRODUCER_INFO";
                    break;
                case TransactionInfo.ID_TransactionInfo :
                    packetTypeStr = "TRANSACTION_INFO";
                    break;
                case BrokerInfo.ID_BrokerInfo :
                    packetTypeStr = "BROKER_INFO";
                    break;
                case ConnectionInfo.ID_ConnectionInfo :
                    packetTypeStr = "CONNECTION_INFO";
                    break;
                case SessionInfo.ID_SessionInfo :
                    packetTypeStr = "SESSION_INFO";
                    break;
                case RemoveSubscriptionInfo.ID_RemoveSubscriptionInfo :
                    packetTypeStr = "DURABLE_UNSUBSCRIBE";
                    break;
                case IntegerResponse.ID_IntegerResponse :
                    packetTypeStr = "INT_RESPONSE_RECEIPT_INFO";
                    break;
                case WireFormatInfo.ID_WireFormatInfo :
                    packetTypeStr = "WIRE_FORMAT_INFO";
                    break;
                case RemoveInfo.ID_RemoveInfo :
                    packetTypeStr = "REMOVE_INFO";
                    break;
                case KeepAliveInfo.ID_KeepAliveInfo :
                    packetTypeStr = "KEEP_ALIVE";
                    break;
            }
            return packetTypeStr;
        }
        
    }
}

