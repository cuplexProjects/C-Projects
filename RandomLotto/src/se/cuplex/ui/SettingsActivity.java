package se.cuplex.ui;

import se.cuplex.model.RndGeneratorController;
import se.cuplex.model.RndGeneratorController.RndAlgorithmHelper;
import android.app.Activity;
import android.content.SharedPreferences;
import android.content.SharedPreferences.OnSharedPreferenceChangeListener;
import android.os.Bundle;
import android.preference.ListPreference;
import android.preference.PreferenceFragment;

public class SettingsActivity extends Activity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        // Display the fragment as the main content.
        getFragmentManager().beginTransaction().replace(android.R.id.content,
                new PrefsFragment()).commit();
    }

    public static class PrefsFragment extends PreferenceFragment implements OnSharedPreferenceChangeListener  {
    	private ListPreference mSetting_number_count;
    	private ListPreference mSetting_grid_max_value;
    	private ListPreference mSetting_rnd_algorithm;
    	
        @Override
        public void onCreate(Bundle savedInstanceState) {
            super.onCreate(savedInstanceState);
            
            //Set Shared preferences name
            getPreferenceManager().setSharedPreferencesName("app_settings_global_key");
            
            // Load the preferences from an XML resource
            addPreferencesFromResource(R.xml.main_preferences);
            
            mSetting_number_count = (ListPreference) getPreferenceScreen().findPreference("setting_number_count");
            mSetting_grid_max_value = (ListPreference) getPreferenceScreen().findPreference("setting_grid_max_value");
            mSetting_rnd_algorithm = (ListPreference) getPreferenceScreen().findPreference("setting_rnd_algorithm");
            
            //Set summary
            if (mSetting_number_count != null)
            	mSetting_number_count.setSummary(mSetting_number_count.getValue());            

            if (mSetting_grid_max_value != null)
            	mSetting_grid_max_value.setSummary(mSetting_grid_max_value.getValue());
            
            if (mSetting_rnd_algorithm != null) {
            	mSetting_rnd_algorithm.setSummary(RndAlgorithmHelper.GetRndAlgorithmName(RndAlgorithmHelper.GetRndAlgorithmFromValue(mSetting_rnd_algorithm.getValue())));
            }
        }
        @Override
		public void onStart() {
            super.onStart();
            this.getPreferenceManager().getSharedPreferences().registerOnSharedPreferenceChangeListener(this);
        }

        @Override
		public void onStop() {
            super.onStop();
            this.getPreferenceManager().getSharedPreferences().unregisterOnSharedPreferenceChangeListener(this);
        }        

		@Override
		public void onSharedPreferenceChanged(
				SharedPreferences sharedPreferences, String key) {
			
            //Set summary
            if (mSetting_number_count != null)
            	mSetting_number_count.setSummary(mSetting_number_count.getValue());            

            if (mSetting_grid_max_value != null)
            	mSetting_grid_max_value.setSummary(mSetting_grid_max_value.getValue());
            
            if (mSetting_rnd_algorithm != null)
            	mSetting_rnd_algorithm.setSummary(RndAlgorithmHelper.GetRndAlgorithmName(RndAlgorithmHelper.GetRndAlgorithmFromValue(mSetting_rnd_algorithm.getValue())));
            
            RndGeneratorController.getInstance().preferencesChanged(key);
		}
    }
}
