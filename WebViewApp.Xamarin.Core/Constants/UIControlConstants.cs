using System;
using Newtonsoft.Json;

namespace WebViewApp.Xamarin.Core.Constants
{
    public enum UIControlTypes
    {
        CHECKBOX_INPUT,
        TEXT_INPUT,
        FORM_TEXT_INPUT,
        SUBMIT_BUTTON,
        NEXT_BUTTON,
        PREVIOUS_BUTTON,
        LOGOUT_BUTTON,
        TEXT_LABEL,
        HIGHLIGHTED_LARGE_LABEL,
        HIGHLIGHTED_SMALL_LABEL,
        NAME_VALUE,
        IMAGE,
        DATE_INPUT,
        RADIO_INPUT,
        NONE
    };

    public enum UIControlPosition
    {
        BOTTOM, TOP, CENTER, RIGHT, LEFT, NONE,
    }

    public enum UIControlValidationType
    {
        ALLWAYS_ON, AllREQUIRED, ANYOPTIONAL, AllREQUIRED_AND_ANYOPTIONAL, AllREQUIRED_OR_ANYOPTIONAL, NONE,
    }

    public enum UIValidationValueTypes
    {
        TEXT,
        INTEGER,
        INTEGER_POS,
        INTEGER_NEG,
        DOUBLE,
        DOUBLE_POS,
        DOUBLE_NEG,
        DATE,
        TIME,
        TIMESTAMP,
        PHONENUMBER,
        EMAILADDRESS,
        URL,
        NONE
    }

    public static class DataDisplayConstants
    {
        public const string TimeFormat = "{0:HH:mm  }";
        public const string DateFormat = "{0:HH:mm, dd MMMM yyyy  }";
    }
}
