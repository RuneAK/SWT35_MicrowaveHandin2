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
using MicrowaveOvenClasses.Boundary;
using Timer = MicrowaveOvenClasses.Boundary.Timer;

namespace Microwave.Test.Integration
{
	[TestFixture]
	public class IT1_Button_Door
	{
        // Substitutes
        private IDisplay _display;
	    private ILight _light;
	    private ITimer _timer;
	    private IPowerTube _powerTube;
	    private ICookController _cookController;
	    private IUserInterface _userInterface;

        // Units under test
        private IButton _uut_powerButton;
		private IButton _uut_timeButton;
		private IButton _uut_startCancelButton;
		private IDoor _uut_door;

        [SetUp]
		public void SetUp()
		{
            // Substitute setup
			_display = Substitute.For<IDisplay>();
			_light = Substitute.For<ILight>();
		    _timer = Substitute.For<ITimer>();
			_powerTube = Substitute.For<IPowerTube>();
		    _cookController = Substitute.For<ICookController>();
		    _userInterface = Substitute.For<IUserInterface>();
			// _cookController.UI = _userInterface;

            // Uut setup
		    _uut_powerButton = new Button();
		    _uut_timeButton = new Button();
		    _uut_startCancelButton = new Button();
		    _uut_door = new Door();
        }

	    [Test]
	    public void Door_Open_UserInterfaceIsReady_LightOn()
	    {
	        _uut_door.Open();
	        _light.Received().TurnOn();
	    }

	    [Test]
	    public void Door_Close_DoorIsOpen_LightOff()
	    {
	        _uut_door.Open();
	        _uut_door.Close();
	        _light.Received().TurnOff();
	    }

	    public void Door_Open_IsCooking_CookingStops()
	    {
	        _uut_powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
	        _uut_timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
	        _uut_startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

	        _uut_door.Open();
	        _cookController.Received().Stop();
	        _display.Received().Clear();
	        _light.Received().TurnOff();
	    }
	}
}
