using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WebIP
{
	public class XMLObject
	{
		public string k1_A { get; set; }
		public string k2_B { get; set; }
		public string x0_C { get; set; }
		public string k3_D { get; set; }
		public string k4_E { get; set; }
		public string y0_F { get; set; }
		public string cm { get; set; }

		public XMLObject()
		{
			this.k1_A = "k1_A nothing";
			this.k2_B = "k2_B nothing";
			this.x0_C = "x0_C nothing";
			this.k3_D = "k3_D nothing";
			this.k4_E = "k4_E nothing";
			this.y0_F= "y0_F nothing";
			this.cm = "cm nothing";
		}			
	}
}
