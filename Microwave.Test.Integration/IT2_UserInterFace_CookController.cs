using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;

namespace Microwave.Test.Integration
{
	[TestFixture]
	public class IT2_UserInterFace_CookController
	{

		private IUserInterface _userInterface;

		private IButton _powerButton;
		private IButton _timeButton;
		private IButton _startCancelButton;

		private IDoor _door;

		private IDisplay _display;
		private ILight _light;

		private CookController _cookController;

		private ITimer _timer;
		private IPowerTube _powerTube;

		[SetUp]
		public void SetUp()
		{
			_powerButton = Substitute.For<IButton>();
			_timeButton = Substitute.For<IButton>();
			_startCancelButton = Substitute.For<IButton>();

			_door = Substitute.For<IDoor>();

			_display = Substitute.For<IDisplay>();
			_light = Substitute.For<ILight>();

			_timer = Substitute.For<ITimer>();
			_powerTube = Substitute.For<IPowerTube>();

			_cookController = new CookController(_timer, _display, _powerTube);
			_userInterface = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light, _cookController);
			_cookController.UI = _userInterface;
		}

		[Test]
		//Useless? Is in UserInterface unit test
		public void OnDoorOpenClose_Light_OnOff()
		{
			_door.Opened += Raise.EventWith(this, EventArgs.Empty);
			_light.Received(1).TurnOn();
			_door.Closed += Raise.EventWith(this, EventArgs.Empty);
			_light.Received(1).TurnOff();
		}

		[Test]
		//Useless? Is in UserInterface unit test
		public void SetPowerTest()
		{
			_powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
			_display.Received(1).ShowPower(50);
			_powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
			_display.Received(1).ShowPower(100);
			_powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
			_display.Received(1).ShowPower(150);
		}

		[Test]
		//Useless? Is in UserInterface unit test
		public void SetTimeTest()
		{
			_powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
			_display.Received(1).ShowPower(50);
			_timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
			_display.Received(1).ShowTime(1,0);
			_timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
			_display.Received(1).ShowTime(2, 0);
			_timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
			_display.Received(1).ShowTime(3, 0);
		}

		[Test]
		public void StartCookingTest()
		{
			SetTimeTest();
			_startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
			_light.Received(1).TurnOn();
			_powerTube.Received(1).TurnOn(50);
			_timer.Received(1).Start(3*60);
		}

		[Test]
		//Useless? Is in UserInterface unit test
		public void SetPowerCancelled()
		{
			SetPowerTest();
			_startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
			_light.Received(1).TurnOff();
			_display.Received(1).Clear();
			_powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
			_display.Received(2).ShowPower(50);
		}

		[Test]
		//Useless? Is in UserInterface unit test
		public void SetPowerDoorOpened()
		{
			SetPowerTest();
			_door.Opened += Raise.EventWith(this, EventArgs.Empty);
			_light.Received(1).TurnOn();
			_display.Received(1).Clear();
			_powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
			_display.Received(1).ShowPower(50);
		}

		[Test]
		//Useless? Is in UserInterface unit test
		public void SetTimeDoorOpened()
		{
			SetTimeTest();
			_door.Opened += Raise.EventWith(this, EventArgs.Empty);
			_light.Received(1).TurnOn();
			_display.Received(1).Clear();
			_powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
			_display.Received(1).ShowPower(50);
			_timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
			_display.Received(1).ShowTime(1, 0);
		}

		[Test]
		public void CookingCancelled()
		{
			StartCookingTest();
			_startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
			_powerTube.Received(1).TurnOff();
			_timer.Received(1).Stop();
			_light.Received(1).TurnOff();
			_display.Received(1).Clear();
			_powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
			_display.Received(2).ShowPower(50);
			_timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
			_display.Received(2).ShowTime(1, 0);
		}

		[Test]

		public void CookingDoorOpened()
		{
			StartCookingTest();
			_door.Opened += Raise.EventWith(this, EventArgs.Empty);
			_powerTube.Received(1).TurnOff();
			_timer.Received(1).Stop();
			_light.Received(1).TurnOn();
			_display.Received(1).Clear();
			_door.Closed += Raise.EventWith(this, EventArgs.Empty);
			_powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
			_display.Received(2).ShowPower(50);
			_timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
			_display.Received(2).ShowTime(1, 0);
		}

		[Test]
		public void CookingTimeTick()
		{
			StartCookingTest();
			_timer.TimerTick += Raise.EventWith(this, EventArgs.Empty);
			_display.Received(1).ShowTime((3*60)/60, (3*60)%60);
		}

		[Test]
		public void CookingFinished()
		{
			StartCookingTest();
			_timer.Expired += Raise.EventWith(this, EventArgs.Empty);
			_powerTube.Received(1).TurnOff();
			_display.Received(1).Clear();
			_light.Received(1).TurnOff();
			_powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
			_display.Received(2).ShowPower(50);
			_timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
			_display.Received(2).ShowTime(1, 0);
		}
	}
}
