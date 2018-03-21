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
	[TestFixture]
	public class IT2_UserInterFace_CookController
	{
		//Stubs
		private IDisplay _display;
		private ILight _light;
		private ITimer _timer;
		private IPowerTube _powerTube;

		//Real
		private IButton _powerButton;
		private IButton _timeButton;
		private IButton _startCancelButton;
		private IDoor _door;

		//UUT
		private IUserInterface _uut_userInterface;
		private CookController _uut_cookController;

		[SetUp]
		public void SetUp()
		{
			//Stubs
			_display = Substitute.For<IDisplay>();
			_light = Substitute.For<ILight>();
			_timer = Substitute.For<ITimer>();
			_powerTube = Substitute.For<IPowerTube>();

			//Real
			_powerButton = new Button();
			_timeButton = new Button();
			_startCancelButton = new Button();
			_door = new Door();

			//UUT
			_uut_cookController = new CookController(_timer, _display, _powerTube);
			_uut_userInterface = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light, _uut_cookController);
			_uut_cookController.UI = _uut_userInterface;
		}

		[Test]
		public void UI_OnDoorOpenClose_Light_OnOff()
		{
			_door.Open();
			_light.Received(1).TurnOn();
			_door.Close();
			_light.Received(1).TurnOff();
		}

		[Test]
		public void UI_SetPowerTest_DisplayShowsPower()
		{
			_powerButton.Press();
			_display.Received(1).ShowPower(50);
			_powerButton.Press();
			_display.Received(1).ShowPower(100);
			_powerButton.Press();
			_display.Received(1).ShowPower(150);
		}

		[Test]
		public void UI_PowerOver700_PowerResets()
		{
			UI_SetPowerTest_DisplayShowsPower();
			for (int i = 150; i < 700; i += 50)
			{
				_powerButton.Press();
				_display.Received(1).ShowPower(i);
			}
			_powerButton.Press();
			_display.ShowPower(50);
		}

		[Test]
		public void UI_SetTimeTest_DisplayShowsTime()
		{
			_powerButton.Press();
			_display.Received(1).ShowPower(50);
			_timeButton.Press();
			_display.Received(1).ShowTime(1,0);
			_timeButton.Press();
			_display.Received(1).ShowTime(2, 0);
			_timeButton.Press();
			_display.Received(1).ShowTime(3, 0);
		}

		[Test]
		public void UI_CC_StartCookingTest_LightPowerTimerOn()
		{
			UI_SetTimeTest_DisplayShowsTime();
			_startCancelButton.Press();
			_light.Received(1).TurnOn();
			_powerTube.Received(1).TurnOn(7);
			_timer.Received(1).Start(3*60);
		}

		[Test]
		public void UI_SetPowerCancelled_LightOffDisplayClear()
		{
			UI_SetPowerTest_DisplayShowsPower();
			_startCancelButton.Press();
			_light.Received(1).TurnOff();
			_display.Received(1).Clear();
			_powerButton.Press();
			_display.Received(2).ShowPower(50);
		}

		[Test]
		public void UI_SetPowerDoorOpened_LightOnDisplayClear()
		{
			UI_SetPowerTest_DisplayShowsPower();
			_door.Open();
			_light.Received(1).TurnOn();
			_display.Received(1).Clear();
			_powerButton.Press();
			_display.Received(1).ShowPower(50);
		}
		
		[Test]
		public void UI_SetTimeDoorOpened_LightOnDisplayClear()
		{
			UI_SetTimeTest_DisplayShowsTime();
			_door.Open();
			_light.Received(1).TurnOn();
			_display.Received(1).Clear();
			_powerButton.Press();
			_display.Received(1).ShowPower(50);
			_timeButton.Press();
			_display.Received(1).ShowTime(1, 0);
		}

		[Test]
		public void UI_CC_CookingCancelled_LightOffDisplayClearPowerTubeOffTimerStop()
		{
			UI_CC_StartCookingTest_LightPowerTimerOn();
			_startCancelButton.Press();
			_powerTube.Received(1).TurnOff();
			_timer.Received(1).Stop();
			_light.Received(1).TurnOff();
			_display.Received(1).Clear();
			_powerButton.Press();
			_display.Received(2).ShowPower(50);
			_timeButton.Press();
			_display.Received(2).ShowTime(1, 0);
		}

		[Test]

		public void UI_CC_CookingDoorOpened_LightOnDisplayClearPowerTubeOffTimerStop()
		{
			UI_CC_StartCookingTest_LightPowerTimerOn();
			_door.Open();
			_powerTube.Received(1).TurnOff();
			_timer.Received(1).Stop();
			_light.Received(1).TurnOn();
			_display.Received(1).Clear();
			_door.Close();
			_powerButton.Press();
			_display.Received(2).ShowPower(50);
			_timeButton.Press();
			_display.Received(2).ShowTime(1, 0);
		}

		[Test]
		public void CC_CookingTimeTick_DisplayTime()
		{
			UI_CC_StartCookingTest_LightPowerTimerOn();
			_timer.TimerTick += Raise.EventWith(this, EventArgs.Empty);
			_display.Received(1).ShowTime((3*60)/60, (3*60)%60);
		}

		[Test]
		public void UI_CC_CookingFinished_PowerTubeOffDisplayClearLightOff()
		{
			UI_CC_StartCookingTest_LightPowerTimerOn();
			_timer.Expired += Raise.EventWith(this, EventArgs.Empty);
			_powerTube.Received(1).TurnOff();
			_display.Received(1).Clear();
			_light.Received(1).TurnOff();
			_powerButton.Press();
			_display.Received(2).ShowPower(50);
			_timeButton.Press();
			_display.Received(2).ShowTime(1, 0);
		}
	}
}
