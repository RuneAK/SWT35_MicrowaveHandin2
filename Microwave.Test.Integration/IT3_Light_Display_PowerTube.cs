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

namespace Microwave.Test.Integration
{
	class IT3_Light_Display_PowerTube
	{
		//Stubs
		
		private ITimer _timer;
		private IOutput _output;


		//Real
		private IButton _powerButton;
		private IButton _timeButton;
		private IButton _startCancelButton;
		private IDoor _door;
		private IUserInterface _userInterface;
		private CookController _cookController;

		//UUT
		private IDisplay _uut_display;
		private ILight _uut_light;
		private IPowerTube _uut_powerTube;
		
		[SetUp]
		public void SetUp()
		{
			//Stubs
			_timer = Substitute.For<ITimer>();
			_output = Substitute.For<IOutput>();

			//UUT
			_uut_display = new Display(_output);
			_uut_light = new Light(_output);
			_uut_powerTube = new PowerTube(_output);

			//Real
			_powerButton = new Button();
			_timeButton = new Button();
			_startCancelButton = new Button();
			_door = new Door();
			_cookController = new CookController(_timer, _uut_display, _uut_powerTube);
			_userInterface = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _uut_display, _uut_light, _cookController);
			_cookController.UI = _userInterface;
		}


		[Test]
		public void Light_OnDoorOpenClose_OutputLight_OnOff()
		{
			_door.Open();
			_output.Received(1).OutputLine("Light is turned on");
			_door.Close();
			_output.Received(1).OutputLine("Light is turned off");
		}

		[Test]
		//Useless? Is in UserInterface unit test
		public void Display_SetPowerTest_OutputPower()
		{
			_powerButton.Press();
			_output.Received(1).OutputLine("Display shows: 50 W");
			_powerButton.Press();
			_output.Received(1).OutputLine("Display shows: 100 W");
			_powerButton.Press();
			_output.Received(1).OutputLine("Display shows: 150 W");
		}
		
		[Test]
		//Useless? Is in UserInterface unit test
		public void Display_SetTimeTest_OutputTime()
		{
			_powerButton.Press();
			_output.Received(1).OutputLine("Display shows: 50 W");
			_timeButton.Press();
			_output.Received(1).OutputLine("Display shows: 01:00");
			_timeButton.Press();
			_output.Received(1).OutputLine("Display shows: 02:00");
			_timeButton.Press();
			_output.Received(1).OutputLine("Display shows: 03:00");
		}
		
		[Test]
		public void Light_PowerTube_StartCookingTest_OutputLightPowerOn()
		{
			Display_SetTimeTest_OutputTime();
			_startCancelButton.Press();
			_output.Received(1).OutputLine("Light is turned on");
			_output.Received(1).OutputLine("PowerTube works with 7 %");
		}

		public void Display_Light_SetPowerCancelled_OutputLightOffDisplayClear()
		{
			Display_SetTimeTest_OutputTime();
			_startCancelButton.Press();
			_output.Received(1).OutputLine("Light is turned off");
			_output.Received(1).OutputLine("Display cleared");	
		}

	}
}
