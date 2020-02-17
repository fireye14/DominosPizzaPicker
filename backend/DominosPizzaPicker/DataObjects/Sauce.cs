using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DominosPizzaPicker.Backend.DataObjects
{
    public class Sauce : EntityData
    {        
        public string Name { get; set; }
    }
}