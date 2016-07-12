SDL Web Trace Visualizer
===================


Tool to visualize trace information from SDL Web.

----------


About
-------------

Since SDL Web 8, the Content Manager backend generates trace information leveraging Windows native Event Tracing for Windows (**ETW**) technology. The information is available in a backward compatible manner using *Start-TcmTracing* and *Stop-TcmTracing* command line which generates a verbose text file. However, analyzing this information in this textual form is extremely complex, if not impossible. Hence, this tool was created, which helps you to analyze the trace information in a graphical manner, where you will be able to navigate through the call chain interactively.

> **Features:**

> - Ability to trace in realtime
> - Ability to visualize historical trace data, as long as it was collected in **ETL** format

#### Start a new Live Trace Session

Go to **File** menu then select **Start New Trace Session**

#### Open up a historical ETL file

Go to **File** menu then select **Open Tracer From ETL File**

#### Configuration
The following options are available, and each item is explained in line.

Channels
:	Default Channel
:	Content Manager Channel
:	Topology Manager Channel

Keywords
:	Public
	> Calls which result in direct invocation of the API and UI.
:	Public Indirect
	> Call which are to Public APIs, but the kernel, but indirectly done due to invocation of another Public API.
:	Internal.
	> All internal methods invoked excluding Database.
:	Database
	> Internal methods which are related to database operation.
:	Extension
	> When the system attempts to invoke an extension.
:	External
	> Any usage of *Tracer* which are not in the SDL product.
:	All

Level
:	Critical
	> Currently Not in Use
:	Error
	> Exceptions occurring in the system.
:	Warning
	> Currently Not in Use
:	Informational
	> All method invocations
:	Verbose
	> Include additional details of methods
	
Process Filter
:	Add Process
	> Enables process filtering, enabling only traces from the selected processes.
:	Clear All Processes
	> Clears all selected process filters.

Building
-------------
Most of the dependencies in the project is configured using NuGet. However, the following assembly needs to be placed manually.
* Tridion.Logging

The assembly can be obtained from a machine where SDL Web Content Manager is installed. It is available in %windir%\Microsoft.NET\assembly.

Support
-------------
SDL Web Trace Visualizer is intended as a toolkit to help SDL Tridion community and is not an officially supported SDL product.

Branches and Contributions
-------------
* master - Represents the latest stable version. This may be a pre-release version
* 8.1 - Represent the version built against SDL Web 8
* 8.2 - Represent the version built against SDL Web 8.2 (Cloud Only)

License
-------------

Copyright (c) 2014-2016 SDL Group.

Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0
Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.