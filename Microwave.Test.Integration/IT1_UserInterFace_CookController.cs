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
    public class IT1_UserInterFace_CookController
	{

		private IUserInterface _userInterface;

		private IButton _powerButton;
		private IButton _timeButton;
		private IButton _startCancelButton;

		private IDoor _door;

		private IDisplay _display;
		private ILight _light;

		private ICookController _cookController;

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

			_cookController = new CookController(_timer, _display, _powerTube, _userInterface);
			_userInterface = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light, _cookController);
		}

		[Test]
		public void OnDoorOpen_Light_Output()
		{
			_door.Opened += Raise.EventWith(this, EventArgs.Empty);
			//_userInterface.Received(1).OnDoorOpened(_door,EventArgs.Empty);
			_light.Received(1).TurnOn();
			_door.Closed += Raise.EventWith(this, EventArgs.Empty);
			//_userInterface.Received(1).OnDoorClosed(_door,EventArgs.Empty);
			_light.Received(1).TurnOff();
			_powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
			//_userInterface.Received(1).OnPowerPressed(_powerButton, EventArgs.Empty);
			_display.Received(1).ShowPower(50);
			_timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
			//_userInterface.Received(1).OnTimePressed(_timeButton, EventArgs.Empty);
			_display.Received(1).ShowTime(1,0);
		}


	}
}
