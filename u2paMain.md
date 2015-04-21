Alternative Software for Top2005+ Universal Programmer



# Introduction #
The TopWin6 software that comes with the Top2005+ Universal Programmer does not use the hardware to the full extent.

This piece software is **not** meant as a replacement for TopWin6, but as a suplement.

# Background #
After having used Top2005+ with TopWin6 for some time, some things started to irritate me:

  * The SRAM-test only support 8bit SRAMs, and not i.e. 4bit.
  * The SRAM-test is very stupidly implemented (for details see my blog-post http://elgensrepairs.blogspot.dk/2012/05/extending-usage-of-top2005-universal.html).
  * Even though the Top2005+ has a 40pins ZIF-socket, reading and programming 16bit EPROMs like 271024-274096 is not supported.

So I started making my own software for reading/writing EPROMs and doing SRAM-tests.

# What have I done then? #
It's a commandline-app coded in C# (it runs on both native .NET and Mono). For USB-communication is uses the GPL-licensed library [LibUsbDotNet](http://sourceforge.net/projects/libusbdotnet/) that in turn uses the library libusb-1.0. It is able to read and program some EPROMs and test some SRAMs.

# So how does it work? #
It's actually really simple. The Top2005+ uses an FPGA to execute the different commands internally. As I don't know how to program FPGAs I have chosen to just use one of the bitstreams that ships with TopWin6. I use a very simple one (ictest.bin) normally used for TTL-testing. That way I only have to send the raw pin-assignments to the Top2005+; all algorithms, timing etc. is implemented in C#. Ofcause I can't do things that needs very precise timing, and a lot of things can be pretty slow.
However it is extremely easy to add support for new EPROMs and SRAMs.

# The process #
See http://elgensrepairs.blogspot.dk/2012/05/extending-usage-of-top2005-universal.html

# Latest Release Notes #
http://code.google.com/p/u2pa/source/browse/Lib/Doc/ReleaseNotes.txt?spec=svne793b6180c40c32c05bbbefe0e80131de4c1ca0a&r=e793b6180c40c32c05bbbefe0e80131de4c1ca0a