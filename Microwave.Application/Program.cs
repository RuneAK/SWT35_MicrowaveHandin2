using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
				ManualResetEvent pause = new ManualResetEvent(false);
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

				output.OutputLine("Main scenario");
				output.OutputLine("User opens door");
				door.Open();
				output.OutputLine("User closes door");
				door.Close();
				output.OutputLine("User presses powerbutton 5 times");
				for (int i = 0; i < 5; i++)
				{
					powerButton.Press();
				}
				output.OutputLine("User presses timebutton 1 time");
				timeButton.Press();
				output.OutputLine("User presses startcancelbutton");
				startCancelButton.Press();
				pause.WaitOne(61000);

				output.OutputLine("User opens door while cooking scenario");
				output.OutputLine("User presses powerbutton");
				powerButton.Press();
				output.OutputLine("User presses timebutton");
				timeButton.Press();
				output.OutputLine("User presses startcancelbutton");
				startCancelButton.Press();
				pause.WaitOne(1500);
				output.OutputLine("User opens door");
				door.Open();
				door.Close();

				output.OutputLine("User press startcancelbutton while cooking scenario");
				output.OutputLine("User presses powerbutton");
				powerButton.Press();
				output.OutputLine("User presses timebutton");
				timeButton.Press();
				output.OutputLine("User presses startcancelbutton");
				startCancelButton.Press();
				pause.WaitOne(1500);
				output.OutputLine("User presses startcancelbutton");
				startCancelButton.Press();


				// Wait while the classes, including the timer, do their job
				System.Console.WriteLine("Tast enter når applikationen skal afsluttes");
				System.Console.ReadLine();
			}
		}
	}
}
