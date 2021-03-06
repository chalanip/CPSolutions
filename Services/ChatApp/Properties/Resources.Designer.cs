﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ChatApp.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ChatApp.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error occured when charge from user {0}..
        /// </summary>
        internal static string Error_ChargeFailed {
            get {
                return ResourceManager.GetString("Error_ChargeFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid input. Please type \&quot;CC &lt;code&gt; &lt;message&gt;\&quot; and send SMS again..
        /// </summary>
        internal static string Error_InvalidChatFormat {
            get {
                return ResourceManager.GetString("Error_InvalidChatFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error in code. Please enter correct code and try again..
        /// </summary>
        internal static string Error_InvalidCode {
            get {
                return ResourceManager.GetString("Error_InvalidCode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid input message. Please type correctly..
        /// </summary>
        internal static string Error_InvalidInputMessage {
            get {
                return ResourceManager.GetString("Error_InvalidInputMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error occured while saving user {0} on DB..
        /// </summary>
        internal static string Error_UserSaveFailed {
            get {
                return ResourceManager.GetString("Error_UserSaveFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Thank you for registering our service. Your code is {0}. Share your code with your friend to use our service..
        /// </summary>
        internal static string Info_RegisterSuccess {
            get {
                return ResourceManager.GetString("Info_RegisterSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You have unsubscribed from the App successfully. Thank you for using our service. Your code is no longer valid now..
        /// </summary>
        internal static string Info_UnregisterSuccess {
            get {
                return ResourceManager.GetString("Info_UnregisterSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You have already registered. Your code is {0}. Share your code with your friend to use our service..
        /// </summary>
        internal static string Info_UserAlreadyRegistered {
            get {
                return ResourceManager.GetString("Info_UserAlreadyRegistered", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User is not registered to the service..
        /// </summary>
        internal static string Info_UserNotRegistered {
            get {
                return ResourceManager.GetString("Info_UserNotRegistered", resourceCulture);
            }
        }
    }
}
