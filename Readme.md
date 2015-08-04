# Jcq

[![Build status](https://ci.appveyor.com/api/projects/status/o3gubp3g9hlraqyg/branch/master?svg=true)](https://ci.appveyor.com/project/janmolnar/jcq/branch/master)

Jcq is an OSCAR (Icq/Aim) Client written in C#/WPF.

This project was previously hosted on [CodePlex](https://jcq.codeplex.com).

## Implemented Features
- Logon on Icq Servers
- Downloading Contact Lists (including Contact Details and Avatars)
- Sending & Receiving of Messages (including offline messages)

## Missing Features
- Logon on Aim Servers
- Adding / Removing of Contacts and Groups
- Logout
- many more

## Project Structure

We use a SOA approach defining service contracts to losely couple the different implementation details.
Implementations are mapped to contracts in the microkernel section of the app.config.
For seperation of Ux and Logic we follow the MVVM pattern.

Common functionality (Microkernel Services, Logging, Exception Publication, ...)
- Jcq.Core 
- Jcq.Core.Contracts

Icq Protocol abstraction (Icq Services (high level), Flaps, Snacs, Tcp Communication (low level), ...)
- Jcq.IcqProtocol
- Jcq.IcqProtocol.Contracts
- Jcq.IcqProtocol.DataTypes

Identity Management (Identities, Credentials, Avatars, ...)
-  Jcq.IdentityManager

User Interface (ViewModels, Views, Styles, ...)
- Jcq.Ux.Main
- Jcq.Ux.ViewModel
- Jcq.Wpf.CommonExtenders
