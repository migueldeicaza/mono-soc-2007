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
using Stomp;
using NMS;
using NUnit.Framework;
using System;

namespace Stomp
{
	[ TestFixture ]
    public class MapMessageTest : NMS.Test.MapMessageTest
    {
		public override void SendAndSyncReceive()
        {
            // TODO disable test
        }
		
        protected override IConnectionFactory CreateConnectionFactory()
        {
            return new ConnectionFactory();
        }
		
        protected override void AssertValidMessage(IMessage message)
        {
            Console.WriteLine("Received MapMessage: " + message);
			
            Assert.IsTrue(message is IMapMessage, "Did not receive a MapMessage!");
		}
    }
}


