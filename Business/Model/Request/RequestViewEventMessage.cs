﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace WX.Model
{
    public class RequestViewEventMessage : RequestEventMessage
    {
        public RequestViewEventMessage(XElement xml)
            : base(xml)
        {
            EventKey = xml.Element("EventKey").Value;     
        }

        public string EventKey { get; set; }
    }
}
