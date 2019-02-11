<%@ Page language="c#" Codebehind="langs-js.aspx.cs" AutoEventWireup="True" Inherits="WebMail.langs_js" validateRequest="false" %>

var Lang = {
<%=OutputLangsConsts()%>
}

var $Monthes = Array();
$Monthes[0]  = Lang.Month;
$Monthes[1]  = Lang.January;
$Monthes[2]  = Lang.February;
$Monthes[3]  = Lang.March;
$Monthes[4]  = Lang.April;
$Monthes[5]  = Lang.May;
$Monthes[6]  = Lang.June;
$Monthes[7]  = Lang.July;
$Monthes[8]  = Lang.August;
$Monthes[9]  = Lang.September;
$Monthes[10] = Lang.October;
$Monthes[11] = Lang.November;
$Monthes[12] = Lang.December;
Lang.Monthes = $Monthes;

var Charsets = [
				{ Name: Lang.CharsetDefault, Value: '0' },
				{ Name: Lang.CharsetArabicAlphabetISO, Value: '28596' },
				{ Name: Lang.CharsetArabicAlphabet, Value: '1256' },
				{ Name: Lang.CharsetBalticAlphabetISO, Value: '28594' },
				{ Name: Lang.CharsetBalticAlphabet, Value: '1257' },
				{ Name: Lang.CharsetCentralEuropeanAlphabetISO, Value: '28592' },
				{ Name: Lang.CharsetCentralEuropeanAlphabet, Value: '1250' },
				{ Name: Lang.CharsetChineseSimplifiedEUC, Value: '51936' },
				{ Name: Lang.CharsetChineseSimplifiedGB, Value: '936' },
				{ Name: Lang.CharsetChineseTraditional, Value: '950' },
				{ Name: Lang.CharsetCyrillicAlphabetISO, Value: '28595' },
				{ Name: Lang.CharsetCyrillicAlphabetKOI8R, Value: '20866' },
				{ Name: Lang.CharsetCyrillicAlphabet, Value: '1251' },
				{ Name: Lang.CharsetGreekAlphabetISO, Value: '28597' },
				{ Name: Lang.CharsetGreekAlphabet, Value: '1253' },
				{ Name: Lang.CharsetHebrewAlphabetISO, Value: '28598' },
				{ Name: Lang.CharsetHebrewAlphabet, Value: '1255' },
				{ Name: Lang.CharsetJapanese, Value: '50220' },
				{ Name: Lang.CharsetJapaneseShiftJIS, Value: '932' },
				{ Name: Lang.CharsetKoreanEUC, Value: '949' },
				{ Name: Lang.CharsetKoreanISO, Value: '50225' },
				{ Name: Lang.CharsetLatin3AlphabetISO, Value: '28593' },
				{ Name: Lang.CharsetTurkishAlphabet, Value: '1254' },
				{ Name: Lang.CharsetUniversalAlphabetUTF7, Value: '65000' },
				{ Name: Lang.CharsetUniversalAlphabetUTF8, Value: '65001' },
				{ Name: Lang.CharsetVietnameseAlphabet, Value: '1258'},
				{ Name: Lang.CharsetWesternAlphabetISO, Value: '28591' },
				{ Name: Lang.CharsetWesternAlphabet, Value: '1252' }
				];

var TimeOffsets = [
					{ Name: Lang.TimeDefault, Value: '0' },
					{ Name: '(GMT -12:00) ' + Lang.TimeEniwetok, Value: '1' },
					{ Name: '(GMT -11:00) ' + Lang.TimeMidwayIsland, Value: '2' },
					{ Name: '(GMT -10:00) ' + Lang.TimeHawaii, Value: '3' },
					{ Name: '(GMT -09:00) ' + Lang.TimeAlaska, Value: '4' },
					{ Name: '(GMT -08:00) ' + Lang.TimePacific, Value: '5' },
					{ Name: '(GMT -07:00) ' + Lang.TimeArizona, Value: '6' },
					{ Name: '(GMT -07:00) ' + Lang.TimeMountain, Value: '7' },
					{ Name: '(GMT -06:00) ' + Lang.TimeCentralAmerica, Value: '8' },
					{ Name: '(GMT -06:00) ' + Lang.TimeCentral, Value: '9' },
					{ Name: '(GMT -06:00) ' + Lang.TimeMexicoCity, Value: '10' },
					{ Name: '(GMT -06:00) ' + Lang.TimeSaskatchewan, Value: '11' },
					{ Name: '(GMT -05:00) ' + Lang.TimeIndiana, Value: '12' },
					{ Name: '(GMT -05:00) ' + Lang.TimeEastern, Value: '13' },
					{ Name: '(GMT -05:00) ' + Lang.TimeBogota, Value: '14' },
					{ Name: '(GMT -04:00) ' + Lang.TimeSantiago, Value: '15' },
					{ Name: '(GMT -04:00) ' + Lang.TimeCaracas, Value: '16' },
					{ Name: '(GMT -04:00) ' + Lang.TimeAtlanticCanada, Value: '17' },
					{ Name: '(GMT -03:30) ' + Lang.TimeNewfoundland, Value: '18' },
					{ Name: '(GMT -03:00) ' + Lang.TimeGreenland, Value: '19' },
					{ Name: '(GMT -03:00) ' + Lang.TimeBuenosAires, Value: '20' },
					{ Name: '(GMT -03:00) ' + Lang.TimeBrasilia, Value: '21' },
					{ Name: '(GMT -02:00) ' + Lang.TimeMidAtlantic, Value: '22' },
					{ Name: '(GMT -01:00) ' + Lang.TimeCapeVerde, Value: '23' },
					{ Name: '(GMT -01:00) ' + Lang.TimeAzores, Value: '24' },
					{ Name: '(GMT) ' + Lang.TimeMonrovia, Value: '25' },
					{ Name: '(GMT) ' + Lang.TimeGMT, Value: '26' },
					{ Name: '(GMT +01:00) ' + Lang.TimeBerlin, Value: '27' },
					{ Name: '(GMT +01:00) ' + Lang.TimePrague, Value: '28' },
					{ Name: '(GMT +01:00) ' + Lang.TimeParis, Value: '29' },
					{ Name: '(GMT +01:00) ' + Lang.TimeSarajevo, Value: '30' },
					{ Name: '(GMT +01:00) ' + Lang.TimeWestCentralAfrica, Value: '31' },
					{ Name: '(GMT +02:00) ' + Lang.TimeAthens, Value: '32' },
					{ Name: '(GMT +02:00) ' + Lang.TimeEasternEurope, Value: '33' },
					{ Name: '(GMT +02:00) ' + Lang.TimeCairo, Value: '34' },
					{ Name: '(GMT +02:00) ' + Lang.TimeHarare, Value: '35' },
					{ Name: '(GMT +02:00) ' + Lang.TimeHelsinki, Value: '36' },
					{ Name: '(GMT +02:00) ' + Lang.TimeIsrael, Value: '37' },
					{ Name: '(GMT +03:00) ' + Lang.TimeBaghdad, Value: '38' },
					{ Name: '(GMT +03:00) ' + Lang.TimeArab, Value: '39' },
					{ Name: '(GMT +03:00) ' + Lang.TimeMoscow, Value: '40' },
					{ Name: '(GMT +03:00) ' + Lang.TimeEastAfrica, Value: '41' },
					{ Name: '(GMT +03:30) ' + Lang.TimeTehran, Value: '42' },
					{ Name: '(GMT +04:00) ' + Lang.TimeAbuDhabi, Value: '43' },
					{ Name: '(GMT +04:00) ' + Lang.TimeCaucasus, Value: '44' },
					{ Name: '(GMT +04:30) ' + Lang.TimeKabul, Value: '45' },
					{ Name: '(GMT +05:00) ' + Lang.TimeEkaterinburg, Value: '46' },
					{ Name: '(GMT +05:00) ' + Lang.TimeIslamabad, Value: '47' },
					{ Name: '(GMT +05:30) ' + Lang.TimeBombay, Value: '48' },
					{ Name: '(GMT +05:45) ' + Lang.TimeNepal, Value: '49' },
					{ Name: '(GMT +06:00) ' + Lang.TimeAlmaty, Value: '50' },
					{ Name: '(GMT +06:00) ' + Lang.TimeDhaka, Value: '51' },
					{ Name: '(GMT +06:00) ' + Lang.TimeSriLanka, Value: '52' },
					{ Name: '(GMT +06:30) ' + Lang.TimeRangoon, Value: '53' },
					{ Name: '(GMT +07:00) ' + Lang.TimeBangkok, Value: '54' },
					{ Name: '(GMT +07:00) ' + Lang.TimeKrasnoyarsk, Value: '55' },
					{ Name: '(GMT +08:00) ' + Lang.TimeBeijing, Value: '56' },
					{ Name: '(GMT +08:00) ' + Lang.TimeIrkutsk, Value: '57' },
					{ Name: '(GMT +08:00) ' + Lang.TimeSingapore, Value: '58' },
					{ Name: '(GMT +08:00) ' + Lang.TimePerth, Value: '59' },
					{ Name: '(GMT +08:00) ' + Lang.TimeTaipei, Value: '60' },
					{ Name: '(GMT +09:00) ' + Lang.TimeTokyo, Value: '61' },
					{ Name: '(GMT +09:00) ' + Lang.TimeSeoul, Value: '62' },
					{ Name: '(GMT +09:00) ' + Lang.TimeYakutsk, Value: '63' },
					{ Name: '(GMT +09:30) ' + Lang.TimeAdelaide, Value: '64' },
					{ Name: '(GMT +09:30) ' + Lang.TimeDarwin, Value: '65' },
					{ Name: '(GMT +10:00) ' + Lang.TimeBrisbane, Value: '66' },
					{ Name: '(GMT +10:00) ' + Lang.TimeSydney, Value: '67' },
					{ Name: '(GMT +10:00) ' + Lang.TimeGuam, Value: '68' },
					{ Name: '(GMT +10:00) ' + Lang.TimeHobart, Value: '69' },
					{ Name: '(GMT +10:00) ' + Lang.TimeVladivostock, Value: '70' },
					{ Name: '(GMT +11:00) ' + Lang.TimeMagadan, Value: '71' },
					{ Name: '(GMT +12:00) ' + Lang.TimeWellington, Value: '72' },
					{ Name: '(GMT +12:00) ' + Lang.TimeFiji, Value: '73' },
					{ Name: '(GMT +13:00) ' + Lang.TimeTonga, Value: '74' }
					];

var DateFormats = [
				{ Name: Lang.DateDefault,  Value: 'Default',  Id: 'date_format_0', LangField: 'DateDefault' },
				{ Name: Lang.DateDDMMYY,   Value: 'DD/MM/YY', Id: 'date_format_1', LangField: 'DateDDMMYY' },
				{ Name: Lang.DateMMDDYY,   Value: 'MM/DD/YY', Id: 'date_format_2', LangField: 'DateMMDDYY' },
				{ Name: Lang.DateDDMonth,  Value: 'DD Month', Id: 'date_format_3', LangField: 'DateDDMonth' },
				{ Name: Lang.DateAdvanced, Value: 'Advanced', Id: 'date_format_4', LangField: 'DateAdvanced' }
				];
