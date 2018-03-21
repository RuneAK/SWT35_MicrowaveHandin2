﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using MicrowaveOvenClasses.Boundary;
using Timer = MicrowaveOvenClasses.Boundary.Timer;

namespace Microwave.Application
{
	namespace Microwave.Application
	{
		class Program
		{
			static void Main(string[] args)
			{
				// Setup all the objects
				var powerButton = new Button();
				var timeButton = new Button();
				var startCancelButton = new Button();
				var door = new Door();
				var output = new Output();
				var display = new Display(output);
				var light = new Light(output);
				var powerTube = new PowerTube(output);
				var timer = new Timer();
				var cookController = new CookController(timer, display, powerTube);
				var userInterFace = new UserInterface(powerButton, timeButton, startCancelButton, door, display,light,cookController);
				cookController.UI = userInterFace;

				// Simulate user activities
				powerButton.Press();
				timeButton.Press();
				startCancelButton.Press();



				// Wait while the classes, including the timer, do their job
				System.Console.WriteLine("Tast enter når applikationen skal afsluttes");
				System.Console.ReadLine();
			}
		}
	}
}
