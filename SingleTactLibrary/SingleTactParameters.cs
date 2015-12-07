//-----------------------------------------------------------------------------
//  Copyright (c) 2015 Pressure Profile Systems
//
//  Licensed under the MIT license. This file may not be copied, modified, or
//  distributed except according to those terms.
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
