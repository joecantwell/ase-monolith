// <copyright company="Action Point Innovation Ltd.">
// Copyright (c) 2013 All Right Reserved
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY 
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace Thirdparty.Api.Contracts
{
    public enum Insurer
    {
        [Display(Name="Alliance Direct")]
        AllianceDirect,

        [Display(Name = "AXA Car Insurance")]
        AxaCar,

        [Display(Name = "Zurich Car Insurance")]
        ZurichCar,

        [Display(Name = "123.ie")]
        OneTwoThree,

        [Display(Name = "FBD Car Insurance")]
        Fbd
    }
}
