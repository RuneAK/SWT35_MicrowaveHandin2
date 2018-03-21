using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Castle.Core.Smtp;
using NSubstitute;
using NUnit.Framework;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using MicrowaveOvenClasses.Boundary;

namespace Microwave.Test.Integration
{
	[TestFixture]
	public class IT1_Button_Door
	{
        //Stubs
        private IDisplay _display;
	    private ILight _light;
	    private ITimer _timer;
	    private IPowerTube _powerTube;
	    private ICookController _cookController;
	    private IUserInterface _userInterface;

        //Units under test
        private IButton _uut_powerButton;
		private IButton _uut_timeButton;
		private IButton _uut_startCancelButton;
		private IDoor _uut_door;

        [SetUp]
		public void SetUp()
		{
            //Stubs
			_display = Substitute.For<IDisplay>();
			_light = Substitute.For<ILight>();
		    _timer = Substitute.For<ITimer>();
			_powerTube = Substitute.For<IPowerTube>();
		    _cookController = Substitute.For<ICookController>();
		    _userInterface = Substitute.For<IUserInterface>();

            //Uut setup
		    _uut_powerButton = new Button();
		    _uut_timeButton = new Button();
		    _uut_startCancelButton = new Button();
		    _uut_door = new Door();
        }

	    [Test]
	    public void Door_Open_UserInterface_OnDoorOpened()
	    {
			_uut_door.Opened += (sender, args) => _userInterface.OnDoorOpened(_uut_door, EventArgs.Empty);
		    _uut_door.Open();
			_userInterface.Received(1).OnDoorOpened(_uut_door,EventArgs.Empty);
		}

	    [Test]
	    public void Door_Close__UserInterface_OnDoorlosed()
	    {
		    _uut_door.Closed += (sender, args) => _userInterface.OnDoorClosed(_uut_door, EventArgs.Empty);
	        _uut_door.Close();
			_userInterface.Received(1).OnDoorClosed(_uut_door, EventArgs.Empty);
		}

		[Test]
	    public void PowerButton_Pressed__UserInterface_OnPowerPressed()
	    {
		    _uut_powerButton.Pressed += (sender, args) => _userInterface.OnPowerPressed(_uut_powerButton, EventArgs.Empty);
			_uut_powerButton.Press();
			_userInterface.Received(1).OnPowerPressed(_uut_powerButton,EventArgs.Empty);
	    }

		[Test]
		public void TimeButton_Pressed__UserInterface_OnTimePressed()
		{
			_uut_timeButton.Pressed += (sender, args) => _userInterface.OnTimePressed(_uut_timeButton, EventArgs.Empty);
			_uut_timeButton.Press();
			_userInterface.Received(1).OnTimePressed(_uut_timeButton, EventArgs.Empty);
		}

		[Test]
		public void StartCancelButton_Pressed__UserInterface_OnStartCancelPressed()
		{
			_uut_startCancelButton.Pressed += (sender, args) => _userInterface.OnStartCancelPressed(_uut_startCancelButton, EventArgs.Empty);
			_uut_startCancelButton.Press();
			_userInterface.Received(1).OnStartCancelPressed(_uut_startCancelButton, EventArgs.Empty);
		}
	}
}
