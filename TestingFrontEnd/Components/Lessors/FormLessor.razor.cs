﻿using Microsoft.AspNetCore.Components;
using Shared.Models;

namespace TestingFrontEnd.Components.Lessors
{
    public partial class FormLessor : ComponentBase
    {
        [Parameter]
        public EventCallback OpenModalLessor { get; set; }
        [Parameter]
        public Lessor? CurrentLessor { get; set; }
        
        public void HandlePost()
        {
            Console.Write("Hello");
        }
    }
}
