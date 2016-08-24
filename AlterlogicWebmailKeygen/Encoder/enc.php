<?php class LicenseManager {
	 const ConstMinus1 = -1;
	 const Const0 = 0;
	 const Const1 = 1;
	 const Const2 = 2;
	 const Const3 = 3;
	 const Const10 = 10;
	 const Const11 = 11;
	 const Const13 = 13;
	 const AFT_LICENSE_KEY_TYPE_VERSION_EXPIRED = 14;	 
	 public $LicenseKey;
	 public $   …… …… ………   ;
	 public $Version;
	 public $   ……… … ………   ;
	 public $   ……… … … …   ;
	 protected $Aurore;
	 public function __construct($strKeyParam, $bAurore = false) {
	 $this->LicenseKey = $strKeyParam;
	 $this->   …… …… ………    = LicenseManager::Const1;
	 $this->Version = LicenseManager::ConstMinus1;
	 $this->   ……… … ………    = 1000;
	 $this->   ……… … … …    = null;
	 $this->   ……… …… ………   = null;
	 $this->Aurore = !!$bAurore;
	 $this->   ……… …  ………  (); }
	 private function    ……… …  ………  () {
		switch (false) {
		case $this->   ………… ……     (): break;
		case $this->   ………… … … …  (): break;
		case $this->   ………… …  ……  (): break;
		default: $this->   ……… …… ……   (); return true; }
		return false;
	 }
	 private function    …………… ……    ($   ………  ……  …  ) {
		if (LicenseManager::Const1 === $   ………  ……  …   || LicenseManager::Const2 === $   ………  ……  …   || LicenseManager::Const3 === $   ………  ……  …  ) {
		$   …… …………………   = SubKeyGenerator::   …………  …………  ($   ………  ……  …  , $this->LicenseKey{	39}	);
		if (in_array($   …… …………………  , array( LicenseManager::Const0, LicenseManager::Const1, LicenseManager::Const2, LicenseManager::Const3 ))) {
			$this->   …… …… ………    = (int) $   …… …………………  ;
			if ($   …… …………………   !== LicenseManager::Const0) {
				$   ……… …  …     = SubKeyGenerator::   …………  …………  ($   ………  ……  …  , $this->LicenseKey{40});
				$   ……… …   ……   = SubKeyGenerator::   …………  …………  ($   ………  ……  …  , $this->LicenseKey{41});
				$   ……… …  ……    = SubKeyGenerator::   …………  …………  ($   ………  ……  …  , $this->LicenseKey{42});

				if (is_numeric($   ……… …  …    ) && is_numeric($   ……… …   ……  ) && is_numeric($   ……… …  ……   )) {
					$this->   ……… … ………    = ($   ……… …  …     * 10 + $   ……… …   ……  ) * pow(10, $   ……… …  ……   ); 
				}
				if ($   …… …………………   === LicenseManager::Const3) {
					$this->   …… …… ………    = LicenseManager::Const13;
					$this->   ……… … … …    = SubKeyGenerator::   ………… …  …   ($   ………  ……  …  , $this->LicenseKey); $this->   ……… …… ………   = $this->   ……… … … …    - time(); 
					if (0 < $this->   ……… …… ………  ) {
					$this->   …… …… ………    = LicenseManager::Const3;
					}
				} 
			} 
			else {
				$this->   …… …… ………    = (int) $   …… …………………  ;
				$this->   ……… … ………    = 0;
			} 
		}
	}

	if (LicenseManager::Const2 !== $   ………  ……  …   && LicenseManager::Const3 !== $   ………  ……  …   && LicenseManager::ConstMinus1 !== $this->   …… …… ………   ) {
		$this->   …… …… ………    = LicenseManager::AFT_LICENSE_KEY_TYPE_VERSION_EXPIRED;
		} 
	}
	 
	private function    ……… …… ……   () {
		$   ………  ……  …   = SubKeyGenerator::GetLicenseTypeAsInt($this->LicenseKey);
		$   …… …… …… …   = SubKeyGenerator::   ………… …  …   ($   ………  ……  …  , $this->LicenseKey); 
		if (false !== $   …… …… …… …   && LicenseManager::ConstMinus1 !== $   ………  ……  …  ) {
			$this->Version = $   ………  ……  …  ;
			if ((bool) (SubKeyGenerator::   …………  …………  ($   ………  ……  …  , $this->LicenseKey{33}) % 2)) {
			$this->   …………… ……    ($   ………  ……  …  ); 
			}
			else {
				$   …… ………………    = time(); $this->   …… …… ………    = LicenseManager::Const11;
				$this->   ……… … … …    = $   …… …… …… …  ;
				$this->   ……… …… ………   = $this->   ……… … … …    - $   …… ………………   ;
				if (0 < $this->   ……… …… ………  ) {
					$this->   …… …… ………    = LicenseManager::Const10;
				}
			}
		}
	 }
	 private function    ………… ……     () {
		return 0;
	}

	private function    ………… … … …  () {
		return true;
	}
	 
	private function    ………… …  ……  () {
		$   …………… …      = ($this->LicenseKey{	35}	 * 7 + 7) % 10;
		return !($this->LicenseKey{36} != $   …………… …      && $this->LicenseKey{37} != $   …………… …      && $this->LicenseKey{38} != $   …………… …     ); 
	}
 }
 	 
function GetRandIntScope_0_9() {
	return rand(0, 9); 
}
	 
class ALInfo extends LicenseManager {
	 
	public function IsValid($   ……… ……   …   = false) {
		return true;
	 }
	 
	 public function IsValidLimit($   ……… …   …   , $   ……… ……   …   = false) {
		return true;	
	 }
	 
	 public function IsAboutToExpire(&$   ………… ……  …  ) {
		return false;
	 }
	 
	 public function ObjValues() {
		return array($this->LicenseKey, $this->   …… …… ………   , $this->   ……… … ………   , $this->   ……… … … …   , $this->   ……… …… ………  , $this->Version); 
	 }
	 
	 public function Generate() {
		return $this->Aurore ? '' :SubKeyGenerator::   ……………  …    (LicenseManager::Const2); 
	 }
	 
	 public function IsAU() {
		return !!$this->Aurore;
	 }
 }
	 
 class SubKeyGenerator {
	 const Const14 = 14;
	 public static function    …………  …………  ($   ………  ……  …  , $   …… ………… ……  ) {
		$   …… …… … ……   = strpos(SubKeyGenerator::   ……… ………  …  ($   ………  ……  …  ), $   …… ………… ……  ); return (false !== $   …… …… … ……  ) ? (int) floor($   …… …… … ……   / 4) : false;
	 }
 
	public static function    ……………  …    ($   ………  ……  …   = LicenseManager::Const2) {
		$   ……… ………………   = SubKeyGenerator::   ……… …… … …  (date("iYsmdH", time() + 3600 * 24 * 30), true); $   …………  … ……   = ''; for ($index = 0, $   …… … ………     = strlen($   ……… ………………  ); $index < $   …… … ………    ;
		$index++) {
		$   …………  … ……   .= SubKeyGenerator::   ………… ………    ($   ………  ……  …  , $   ……… ………………  [$index]); }
		$   ……… …………     = array('', '', '', '', '', '', '', '', '', '', '', '', ''); $   ……………    …   = array("A", "W", "M", "C");
		for ($index = 0;	 $index <= 3;	 $index++) {
		 $   ……… … … ……   = rand(0, 12); 
		 while ($   ……… …………    [$   ……… … … ……  ] != '') {
			 $   ……… … … ……   = $   ……… … … ……   + 1;
			 $   ……… … … ……   = ($   ……… … … ……   > 12) ? 0 : $   ……… … … ……  ;
		 }
		 $   ……… …………    [$   ……… … … ……  ] = $   ……………    …  [$index]; 
		}
		for ($index = 0;	 $index <= 12;	 $index++) {
				$   …………  … ……   .= ($   ……… …………    [$index] != '') ? $   ……… …………    [$index] : SubKeyGenerator::   ………… ………    ($   ………  ……  …  , GetRandIntScope_0_9()); 
		}

		$   …………  … ……   .= SubKeyGenerator::   ………… ………    ($   ………  ……  …  , rand(0, 4) * 2);
		$   ………… ……………   = SubKeyGenerator::   ………… ………    ($   ………  ……  …  , 0). SubKeyGenerator::   ………… ………    ($   ………  ……  …  , GetRandIntScope_0_9()). SubKeyGenerator::   ………… ………    ($   ………  ……  …  , GetRandIntScope_0_9()). SubKeyGenerator::   ………… ………    ($   ………  ……  …  , GetRandIntScope_0_9());
		$   ……… …    …   = GetRandIntScope_0_9(); $   ……………  … …   = ($   ……… …    …   * 7 + 7) % 10;
		$   ………… …… ……   = rand(0, 2); $   …………  …      = (string) $   ……… …    …  ;
		for ($index = 0; $index <= 2; $index++) {
				$   …………  …      .= ($index == $   ………… …… ……  ) ? $   ……………  … …   : GetRandIntScope_0_9(); 
			}
		return SubKeyGenerator::   ……… … …… …  ($   ………  ……  …  ).$   …………  … ……  ."-".$   …………  …     .$   ………… ……………  ."A"; 
	}
 
	public static function    ………… …  …   ($   ………  ……  …  , $strKeyParam) {
			$licenseTypeRetVal = false;

			$   …………… …  …   = substr($strKeyParam, 6, SubKeyGenerator::Const14);
			$   ……… ……  …    = SubKeyGenerator::GenerateInitArray();
			$   ……… ……       = '';
 
	 
	 for ($index = 0;	 $index < SubKeyGenerator::Const14;	 $index++) {
				$   ……… …    …   = (int) SubKeyGenerator::   …………  …………  ($   ………  ……  …  , $   …………… …  …  [$index]);
				$   ……… …    …   = $   ……… …    …   - $   ……… ……  …   [$index];
				$   ……… …    …   = ($   ……… …    …   < 0) ? $   ……… …    …   + 10 : $   ……… …    …  ;
				$   ……… …    …   = ($   ……… …    …   >= 10) ? $   ……… …    …   - 10 : $   ……… …    …  ;
				$   ……… ……       .= (string) $   ……… …    …  ;
	 }
	 
	 if (strlen($   ……… ……      ) === SubKeyGenerator::Const14) {
				$   …… ………… …    = (int) substr($   ……… ……      , 12, 2);
				$   ………  …   …   = (int) substr($   ……… ……      , 0, 2);
				$   ………   ………    = (int) substr($   ……… ……      , 6, 2);
				$   ………     …    = (int) substr($   ……… ……      , 8, 2);
				$   …… …… …      = (int) substr($   ……… ……      , 10, 2);
				$   …… ……… ………   = (int) substr($   ……… ……      , 2, 4);
				$licenseTypeRetVal = gmmktime($   …… ………… …   , $   ………  …   …  , $   ………   ………   , $   ………     …   , $   …… …… …     , $   …… ……… ………  ); 
			}
			
	 return $licenseTypeRetVal;
	}
 
	 public static function GetLicenseTypeAsInt($strKeyParam) {
		 $licenseTypeRetVal = LicenseManager::ConstMinus1;
		 $LicenseKeySegment1 = substr($strKeyParam, 0, 6); 
		 
		 if ("WM700-" === $LicenseKeySegment1) {
					$licenseTypeRetVal = LicenseManager::Const2;
		 }
		 else if ("AU700-" === $LicenseKeySegment1) {
					$licenseTypeRetVal = LicenseManager::Const3;
		 }
		 else if ("WM510-" === $LicenseKeySegment1) {
					$licenseTypeRetVal = LicenseManager::Const1;
		 }
		 else if ("WM500-" === $LicenseKeySegment1) {
					$licenseTypeRetVal = LicenseManager::Const0;
		 }
		 return $licenseTypeRetVal;		 
	 }
 
	 public static function    ……… … …… …  ($   ………  ……  …  ) {
		 $   ………   …… …   = "WM510-"; 
		 if (LicenseManager::Const2 === $   ………  ……  …  ) {
					$   ………   …… …   = "WM700-"; }
		 else if (LicenseManager::Const3 === $   ………  ……  …  ) {
					$   ………   …… …   = "AU700-"; }
		 return $   ………   …… …  ;
	 }
 
	 protected static function    ……… ………  …  ($   ………  ……  …  ) {
		 $   ………   …… …   = "4JUV3HSWIT55GR6R2FQXZBLZEP771DNYCM8MAK99"; 
		 if (LicenseManager::Const2 === $   ………  ……  …  ) {
					$   ………   …… …   = "2FQX3HSW4JUVIT55GR6RAK99ZBLZEP771DNYCM8M"; }
		 else if (LicenseManager::Const3 === $   ………  ……  …  ) {
					$   ………   …… …   = "3HSW2FQXZBLZCM8MAK99IT55EP771DNYGR6R4JUV"; }
		 return $   ………   …… …  ;
	 }
 
	protected static function    ………… ………    ($   ………  ……  …  , $   …… ……  ………  ) {
		$   …… ……  ………   = (0 < $   …… ……  ………   && $   …… ……  ………   < 10) ? $   …… ……  ………   : 0;
		$   …… ……  ………   = (int) (($   …… ……  ………   * 4) + rand(0, 3)); $s   ……………   ……   = SubKeyGenerator::   ……… ………  …  ($   ………  ……  …  ); 
		return $s   ……………   ……  {$   …… ……  ………  }; 
	}
 
	protected static function    ……… …… … …  ($   ………  … ……   , $   …… ……… … …  ) {
		 $   ……… ……  …    = self::GenerateInitArray(); $   ………   …… …   = ''; 
		 for ($index = 0, $   …… … ………     = strlen($   ………  … ……   ); $index < $   …… … ………    ; $index++) {
			 $   ……… …    …   = $   ………  … ……   {$index}; 
			 if (true === $   …… ……… … …  ) {
						$   ……… …    …   += $   ……… ……  …   [$index]; 
					}
					else {
						$   ……… …    …   -= $   ……… ……  …   [$index]; 
			 }
			 $   ……… …    …   = ($   ……… …    …   < 0) ? $   ……… …    …   + 10 : $   ……… …    …  ;
			 $   ……… …    …   = ($   ……… …    …   >= 10) ? $   ……… …    …   - 10 : $   ……… …    …  ;
			 $   ………   …… …   .= (string) $   ……… …    …  ;
		 }
		 return $   ………   …… …  ;
	}
 
	protected static function GenerateInitArray() {
		 $initArray = array(); 
		 for ($index = 0; $index < SubKeyGenerator::Const14; $index++) {
			$   …… ……… …     = (string) round($index * 897); $   …… … ……  …   = (int) $   …… ……… …    {
			strlen($   …… ……… …    ) - 1}; $   …… … ……………   = (string) $index;
			$   …… … ……………   = (int) $   …… … ……………  {
			strlen($   …… … ……………  ) - 1}; $   …… ……  … …   = (($   …… … ……………   <= $   …… … ……  …  ) || $   …… … ……  …   === 0) ? $   …… … ……  …   : "-".$   …… … ……  …  ;
			$initArray[] = (int) $   …… ……  … …  ;
		 }
		 
		 return $initArray;
	}
}