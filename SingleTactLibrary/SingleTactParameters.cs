//-----------------------------------------------------------------------------
//                                                                            
//  Copyright (c) 2015 All Right Reserved                                      
//  Pressure Profile Systems                                                   
//  www.pressureprofile.com                                                    
//  V1.0                                                         
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SingleTactLibrary
{
   public class SingleTactParameters
   {
      public const int ParamLocation = 112;

      /// <summary>
      /// The raw parameters array as found on sensor memory
      /// </summary>
      public byte[] ParametersRaw
      {
         get { return parametersRaw_; }
         set { parametersRaw_ = value; }
      }
      private byte[] parametersRaw_;
   }
}
