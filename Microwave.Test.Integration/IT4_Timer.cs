using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using NSubstitute;
using NUnit.Framework;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using System.Threading;

namespace Microwave.Test.Integration
{
	class IT4_Timer
	{
		//Stubs
		private IOutput _output;


		//Real
		private IButton _powerButton;
		private IButton _timeButton;
		private IButton _startCancelButton;
		private IDoor _door;
		private IUserInterface _userInterface;
		private CookController _cookController;
		private IDisplay _display;
		private ILight _light;
		private IPowerTube _powerTube;

		//UUT
		private ITimer _uut_timer;
		

		[SetUp]
		public void SetUp()
		{
			//Stubs
			_output = Substitute.For<IOutput>();

			//UUT
			_uut_timer = new MicrowaveOvenClasses.Boundary.Timer();

			//Real
			_display = new Display(_output);
			_light = new Light(_output);
			_powerTube = new PowerTube(_output);
			_powerButton = new Button();
			_timeButton = new Button();
			_startCancelButton = new Button();
			_door = new Door();
			_cookController = new CookController(_uut_timer, _display, _powerTube);
			_userInterface = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light, _cookController);
			_cookController.UI = _userInterface;
		}

		[Test]
		public void Timer_TimeTick_OutputShowsTime()
		{

			ManualResetEvent pause = new ManualResetEvent(false);
			_powerButton.Press();
			_timeButton.Press();
			_startCancelButton.Press();
			pause.WaitOne(2100);
			_output.Received(1).OutputLine("Display shows: 00:58");
		}

		[Test]
		public void Timer_FinishCooking_OutputLightOff()
		{
			
			ManualResetEvent pause = new ManualResetEvent(false);
			_powerButton.Press();
			_timeButton.Press();
			_startCancelButton.Press();
			pause.WaitOne(60100);
			_output.Received(1).OutputLine("Light is turned off");
			_output.Received(1).OutputLine("PowerTube turned off");
		}
	}
}
