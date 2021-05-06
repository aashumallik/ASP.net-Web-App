using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirestoneWebTemplate.Classes
{
    [Serializable()]
    public class SampleSpecs
    {        
        private string _Spec  = null;
        private string _Product_Code = null;
        private string _Tread_Code = null;
        private string _spec_desc = null;
        private decimal _weight = 0;
        private decimal _CarWeight = 0;


        public string Spec { get { return _Spec; } set { _Spec = value; } }
        public string ProductCode { get { return _Product_Code; } set { _Product_Code = value; } }
        public string TreadCode { get { return _Tread_Code; } set { _Tread_Code = value; } }
        public string SpecDesc { get { return _spec_desc; } set { _spec_desc = value; } }
        public decimal TireWeight { get { return _weight; } set { _weight = value; } }
        public decimal CarcassWeight { get { return _CarWeight; } set { _CarWeight = value; } }
        

    }
}