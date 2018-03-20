using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using Timer = MicrowaveOvenClasses.Boundary.Timer;

namespace Microwave.Test.Integration
{
	[TestFixture]
	class IT1_Button_Door
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

			_timer = new IT4_Timer();

			_powerTube = Substitute.For<IPowerTube>();

			_cookController = new CookController(_timer, _display, _powerTube);
			_userInterface = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light, _cookController);
			_cookController.UI = _userInterface;
		}

		[Test]

	}
}
