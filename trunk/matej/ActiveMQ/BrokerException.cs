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
using ActiveMQ.Commands;
using System.Text;
using NMS;

namespace ActiveMQ
{
	
	/// <summary>
	/// Exception thrown when the broker returns an error
	/// </summary>
	public class BrokerException : NMSException
    {
        private BrokerError brokerError;
        
		/// <summary>
		/// Generates a nice textual stack trace
		/// </summary>
		public static string StackTraceDump(StackTraceElement[] elements)
		{
			StringBuilder builder = new StringBuilder();
			if (elements != null) 
			{
				foreach (StackTraceElement e in elements) 
				{
					builder.Append("\n " + e.ClassName + "." + e.MethodName + "(" + e.FileName + ":" + e.LineNumber + ")");
				}
			}
			return builder.ToString();
		}
		
        public BrokerException() : base("Broker failed with missing exception log")
        {
        }
        
        public BrokerException(BrokerError brokerError) : base(
            brokerError.ExceptionClass + " : " + brokerError.Message + "\n" + StackTraceDump(brokerError.StackTraceElements))
        {
            this.brokerError = brokerError;
        }
        
        public BrokerError BrokerError
		{
            get {
                return brokerError;
            }
        }
        
        public virtual string JavaStackTrace
        {
            get {
                return brokerError.StackTrace;
            }
        }
        
    }
}


