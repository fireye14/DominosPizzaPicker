using System;
using Microsoft.WindowsAzure.MobileServices;

namespace DominosPizzaPicker.Client.Models
{
    public class Sauce
    {
        public string Id { get; set; }

        public string Name { get; set; }

        // If need to have different name than database column, do this ('text' is table column name)
        //string name;
        //[JsonProperty(PropertyName = "text")]
        //public string Name
        //{
        //    get { return name; }
        //    set { name = value; }
        //}

        [Version]
        public string Version { get; set; }
    }
}
