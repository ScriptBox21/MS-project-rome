//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System;
using System.Threading.Tasks;

namespace Microsoft.ConnectedDevices
{
    public partial class RemoteLauncher
    {
        public static Task<RemoteLaunchUriStatus> LaunchUriAsync(RemoteSystemConnectionRequest connectionRequest, Uri uri)
        {
            var tcs = new TaskCompletionSource<RemoteLaunchUriStatus>();

            try
            { 
                var launchUriListener = new LaunchURIListener();
                launchUriListener.LaunchCompleted += (r) =>
                {
                    tcs.TrySetResult(r);
                };
                
                RemoteLauncher.LaunchUriAsync(connectionRequest, SystemUriToAndroidUri(uri), launchUriListener);
            }
            catch (Exception e)
            {
                tcs.TrySetException(e);
            }

            return tcs.Task;
        }

        private static Android.Net.Uri SystemUriToAndroidUri(System.Uri uri)
        {
            return Android.Net.Uri.Parse(uri.OriginalString);
        }
    }

    internal class LaunchURIListener : Java.Lang.Object, IRemoteLauncherListener
    {
        public event Action<RemoteLaunchUriStatus> LaunchCompleted;

        public void OnCompleted(RemoteLaunchUriStatus p0)
        {
            this.LaunchCompleted?.Invoke(p0);
        }
    }
}