using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChatBusinessObjects
{
    public class ChatMessage : BindableBase
    {
        public int Id { get; set; }
        public String Speaker { get; set; }
        public DateTime EmissionDate { get; set; }
        public String Content { get; set; }
    }
}
