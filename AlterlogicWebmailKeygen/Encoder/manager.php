<?php
/*
License types
{
	number of users
	unlimited
}
expire date

*/



public function GetUserNumberLimitAsString()
	{
		$aInfo = $this->getInfo()->ObjValues();
		$sResult = empty($aInfo[0]) ? 'Empty' : 'Invalid';
		if (isset($aInfo[1], $aInfo[2], $aInfo[5]))
		{
			switch ($aInfo[1])
			{
				case 0:
					$sResult = 'Unlim';
					break;
				case 1:
					$sResult = $aInfo[2].' users, Permanent';
					break;
				case 2:
					$sResult = $aInfo[2].' domains';
					break;
				case 10:
					$sResult = 'Trial';
					if (isset($aInfo[4]))
					{
						$sResult .= ', expires in '.ceil($aInfo[4] / 60 / 60 / 24).' day(s).';
					}
					break;
				case 11:
					$sResult = 'Trial expired.
This license is outdated, please contact AfterLogic to upgrade your license key.';
					break;
				case 3:
					$sResult =  $aInfo[2].' users, Annual';
					if (isset($aInfo[4]))
					{
						$sResult .= ', expires in '.ceil($aInfo[4] / 60 / 60 / 24).' day(s).';
					}
					break;
				case 13:
					$sResult = $aInfo[2].' users, Annual, Expired.
This license is outdated, please contact AfterLogic to upgrade your license key.';
					break;
				case 14:
					$sResult = 'This license is outdated, please contact AfterLogic to upgrade your license key.';
					break;
			}
		}

		return $sResult;
	}
	
/>