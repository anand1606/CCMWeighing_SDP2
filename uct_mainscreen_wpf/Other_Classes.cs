using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uct_main_wpf
{
    class CurrentSetting
    {
        public decimal MinWt { get; set; }
        public decimal MaxWt { get; set; }
        public decimal NomWt { get; set; }
        public decimal Length { get; set; }
        public string Size { get; set; }
        public string Class { get; set; }
        public string Joint { get; set; }
        public string MouldNo { get; set; }


        public CurrentSetting()
        {
            this.MinWt = 0;
            this.MaxWt = 0;
            this.NomWt = 0;
            this.Length = 0;
            this.Size = "";
            this.Class = "";
            this.Joint = "";
            this.MouldNo = "";
        }
    }


    class StreamReceiver
    {
        public string MachineID { get; set; }
        public double ActWt { get; set; }
        public int SrNo { get; set; }
        public string PipeNumber { get; set; }
        public DateTime LogDateTime { get; set; }
        public CurrentSetting Parameters { get; set; }

        public StreamReceiver(CurrentSetting t)
        {
            this.MachineID = "";
            this.ActWt = 0;
            this.Parameters = t;
            this.LogDateTime = DateTime.MinValue;
        }

        public StreamReceiver()
        {
            this.MachineID = "";
            this.ActWt = 0;
            this.LogDateTime = DateTime.MinValue;
        }

    }
}
