/***************************************************************************

Copyright (c) Microsoft Corporation. All rights reserved.
This code is licensed under the Visual Studio SDK license terms.
THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.

***************************************************************************/

using System;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudioTools.VSTestHost;
using Thread = System.Threading.Thread;
using vsBuildState = EnvDTE.vsBuildState;

namespace Microsoft.VisualStudio.Project.IntegrationTests
{
    [TestClass]
    public abstract class BaseTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestInitialize()]
        public virtual void Initialize()
        {
        }

        [TestCleanup()]
        public void MyTestCleanup()
        {
            while (VSTestContext.DTE.Solution.SolutionBuild.BuildState == vsBuildState.vsBuildStateInProgress)
            {
                Thread.Sleep(1000);
            }

            IVsSolution solutionService = VSTestContext.ServiceProvider.GetService(typeof(SVsSolution)) as IVsSolution;
            if (solutionService != null)
            {
                object isOpen;
                ErrorHandler.ThrowOnFailure(solutionService.GetProperty((int)__VSPROPID.VSPROPID_IsSolutionOpen, out isOpen));
                if (Convert.ToBoolean(isOpen))
                {
                    ErrorHandler.ThrowOnFailure(solutionService.CloseSolutionElement((uint)__VSSLNSAVEOPTIONS.SLNSAVEOPT_ForceSave, null, 0));
                }
            }
        }
    }
}
