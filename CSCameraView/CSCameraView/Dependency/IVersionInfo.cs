using System;
using System.Collections.Generic;
using System.Text;

namespace CSCameraView.Dependency
{
    public interface IVersionInfo
    {

		
			/// <summary>
			/// Gets the version name string used in the Application.
			/// </summary>
			string VersionName { get; }

			/// <summary>
			/// Gets the unique version code for the Application.
			/// </summary>
			string VersionCode { get; }
	}
}
