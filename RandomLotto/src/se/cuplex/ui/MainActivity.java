package se.cuplex.ui;

import se.cuplex.model.NotifyListener;
import se.cuplex.model.RandomOrg;
import se.cuplex.model.RndGeneratorController;
import se.cuplex.model.RndGeneratorController.RndAlgorithm;
import se.cuplex.model.UnInitializedException;
import android.os.Bundle;
import android.app.Activity;
import android.app.AlertDialog;
import android.app.Dialog;
import android.app.DialogFragment;
import android.app.FragmentManager;
import android.app.ProgressDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.graphics.PixelFormat;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.Window;
import android.widget.Button;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

public class MainActivity extends Activity implements NotifyListener {

	private LottoView mLottoView;
	private TextView mStatusText;
	private SharedPreferences mPrefs;
	private Button mGenRndSeedButton;
	private ProgressBar mRnd_org_buffer_meter;
	private ProgressDialog mProgressDialog;
	
	public MainActivity() {		
		   
	}
	
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);                
        
        //Set root background color to black
        View someView = findViewById(R.id.button_layout);
        View root = someView.getRootView();
        root.setBackgroundColor(getResources().getColor( android.R.color.black));
        
        mLottoView = (LottoView)findViewById(R.id.lotto_view);
        mGenRndSeedButton = (Button)findViewById(R.id.gen_rnd_seed_button);
        Button clearButton = (Button)findViewById(R.id.clear_button);        
        Button generateButton = (Button)findViewById(R.id.generate_button);        
        mStatusText = (TextView)findViewById(R.id.main_status_label);
        mRnd_org_buffer_meter = (ProgressBar)findViewById(R.id.rnd_org_buffer_meter);
        
        mGenRndSeedButton.setOnClickListener(new GenerateRndSeedButtonListener());
        clearButton.setOnClickListener(new ClearButtonListener());
        generateButton.setOnClickListener(new GenerateButtonListener());
        RndGeneratorController.getInstance().addNotifyListener(this);
        
        mPrefs = MainActivity.this.getSharedPreferences("app_settings_global_key", 0);
        
    	//Only initialize on first launch and then when preferences change!        
    	initPreferences(savedInstanceState == null || RndGeneratorController.getInstance().getCurrentAlgorithm() == RndAlgorithm.None);
    }
    
    @Override
    public void onStart() {
    	super.onStart();
    	
    	//Set visibility of certain controls
    	setVisibility();
    }
    
    @Override
    public void onStop() {
    	super.onStop();
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        getMenuInflater().inflate(R.menu.activity_main, menu);
        
        return true;
    }
    
    @Override
    public boolean onOptionsItemSelected(MenuItem item){    	
    	if (item.getItemId() == R.id.menu_about) {
    		showAboutDialog();
    		    		
    		return true;
    	}
    	else if (item.getItemId() == R.id.menu_joker) {
        	if (!RndGeneratorController.getInstance().isInitialized()) 
        		showUninitializedWarning();
    		else
    			showJokerDialog();    		
    		    		
    		return true;
    	}
    	else if (item.getItemId() == R.id.menu_settings) {
    		startActivity(new Intent(MainActivity.this, SettingsActivity.class));
    	}
    	
    	return false;
    }
    
    @Override
    public void onAttachedToWindow() {
        super.onAttachedToWindow();
        Window window = getWindow();
        window.setFormat(PixelFormat.RGBA_8888);
    }
    
    @Override
    public void onSaveInstanceState(Bundle savedInstanceState) {
      super.onSaveInstanceState(savedInstanceState);
      
      savedInstanceState.putString("StatusText", mStatusText.getText().toString());
      if (mRnd_org_buffer_meter != null) {
	      savedInstanceState.putInt("mRnd_org_buffer_meter.Progress", mRnd_org_buffer_meter.getProgress());
	      savedInstanceState.putInt("mRnd_org_buffer_meter.Max", mRnd_org_buffer_meter.getMax());
      }
    }    
    
    @Override
    public void onRestoreInstanceState(Bundle savedInstanceState) {
      super.onRestoreInstanceState(savedInstanceState);
      
      String statustext = savedInstanceState.getString("StatusText");
      if (statustext != null && mStatusText != null)
    	  mStatusText.setText(statustext);
      
      if (mRnd_org_buffer_meter != null) {
    	  mRnd_org_buffer_meter.setMax(savedInstanceState.getInt("mRnd_org_buffer_meter.Max", 100));
    	  mRnd_org_buffer_meter.setProgress(savedInstanceState.getInt("mRnd_org_buffer_meter.Progress", 0));
      }
    }
    
    private void showAboutDialog() {
        FragmentManager fragmentManager = getFragmentManager();
        AboutDialogFragment newFragment = new AboutDialogFragment();
        newFragment.show(fragmentManager, "about_dialog");
    }
    
    private void showJokerDialog() {
    	FragmentManager fragmentManager = getFragmentManager();
    	JokerDialogFragment newFragment = new JokerDialogFragment();
        newFragment.show(fragmentManager, "joker_dialog");
    }
    
    private void showUninitializedWarning() {
    	if (RndGeneratorController.getInstance().getCurrentAlgorithm() == RndAlgorithm.Blum_Blum_Shub)
    		Toast.makeText(this, R.string.uninitialized_warning, Toast.LENGTH_LONG).show();
    }
    
    private void showReinitializeDialog() {
		DialogFragment dialogFragment = MyAlertDialogFragment.newInstance(R.string.reInitializeDialogMessage, 
				MyAlertDialogFragment.AlertDialogTypes.Reinitialize);
    	dialogFragment.show(getFragmentManager(), "dialog");
    }
    
    private void clearRandomSeed() {
    	if (RndGeneratorController.getInstance().getCurrentAlgorithm()==RndAlgorithm.Blum_Blum_Shub) {    	
    		RndGeneratorController.getInstance().clearDataStructures();
    		mStatusText.setText(R.string.status_init_text);
    	}
    	mLottoView.clearSelectedNumbers();
    }
    
    private void initPreferences(boolean algorithmChanged) {
		int number_count = Integer.parseInt(mPrefs.getString("setting_number_count", "7"));
		int grid_max_value = Integer.parseInt(mPrefs.getString("setting_grid_max_value", "35"));
		int gridSize = (int)Math.ceil(Math.sqrt(grid_max_value));
				
		if (algorithmChanged) {			
			RndAlgorithm algorithm = RndGeneratorController.RndAlgorithmHelper.GetRndAlgorithmFromValue(mPrefs.getString("setting_rnd_algorithm", "1"));			
			mStatusText.setText(R.string.status_init_text);
		
			switch (algorithm) {
				case Blum_Blum_Shub:
					RndGeneratorController.getInstance().setRandomAlgorithm(RndGeneratorController.RndAlgorithm.Blum_Blum_Shub);
					break;
				case Blum_Blum_Shub_Auto_Seed:
					RndGeneratorController.getInstance().setRandomAlgorithm(RndGeneratorController.RndAlgorithm.Blum_Blum_Shub_Auto_Seed);
					break;
				case Random_ORG:
					RndGeneratorController.getInstance().setRandomAlgorithm(RndGeneratorController.RndAlgorithm.Random_ORG);
					mRnd_org_buffer_meter.setMax(RandomOrg.RANDOM_BUFFER_SIZE);
					mRnd_org_buffer_meter.setProgress(RandomOrg.RANDOM_BUFFER_SIZE);					
					break;
				case None:
					break;
			}
		}
		
		mLottoView.setParameters(gridSize, grid_max_value, number_count);
    }
    
    private void setVisibility() {
    	RndAlgorithm algorithm = RndGeneratorController.getInstance().getCurrentAlgorithm();
    	
    	mGenRndSeedButton.setVisibility(View.INVISIBLE);
    	mRnd_org_buffer_meter.setVisibility(View.INVISIBLE);
    	
    	switch(algorithm) {
    		case Blum_Blum_Shub:
    			mGenRndSeedButton.setVisibility(View.VISIBLE);
    			break;    		
    		case Blum_Blum_Shub_Auto_Seed:
    			break;    			
    		case Random_ORG:
    			mRnd_org_buffer_meter.setVisibility(View.VISIBLE);
    			break;    			
    		case None:
				break;
    	}
    }
    
	@Override
	public void onNotify(NotificationTypes notificationType, String message) {
		
		switch(notificationType) {
			case RndInitVectorChanged:
				mStatusText.setText("Initialized " + message);
				RndGeneratorController.getInstance().initRndAlgorithm();
				break;
			case RndOrgCallBegin:
				mStatusText.setText("Fetching data from random.org");
				break;
			case RndOrgCallComplete:
				mStatusText.setText("Data received from Random.org");
				break;
			case RndInitialized:				
				if (RndGeneratorController.getInstance().getCurrentAlgorithm() == RndAlgorithm.Random_ORG) {
					if (Boolean.valueOf(message)) {
						mStatusText.setText("Initialized using Random.org");
						mRnd_org_buffer_meter.setProgress(RandomOrg.RANDOM_BUFFER_SIZE);
					}
					else
						mStatusText.setText("Faild to initialize using Random.org");
					
					if (mProgressDialog != null) {
						mProgressDialog.dismiss();
						mProgressDialog = null;
					}
				}				
				else {
					if (Boolean.valueOf(message))
						mStatusText.setText("Initialized");
					else
						mStatusText.setText("Uninitialized");
				}
				
				break;
			case RndOrgBufferPositionChanged:
				int progress = Integer.parseInt(message);
				mRnd_org_buffer_meter.setProgress(RandomOrg.RANDOM_BUFFER_SIZE - progress);
				break;
			case RndOrgEmptyBuffer:
				if (!RndGeneratorController.getInstance().isInitialized()) {
					showRandomOrgLoadingMessage();
					RndGeneratorController.getInstance().initRndAlgorithm();
				}
				break;
			case PreferencesChanged:
				boolean algChanged = false;
				
				if (message != null && message instanceof String) {
					algChanged = message.contains("setting_rnd_algorithm");
				}
				
				initPreferences(algChanged);
				break;
			default:
				break;
		}
	}
	
	private void showRandomOrgLoadingMessage() {
		mProgressDialog = ProgressDialog.show(this, "Please wait", "Requesting new random bits from Random.org");
	}
	
	
	// ********************************************************************************************
	//		Sub Classes
	// ********************************************************************************************
    private class GenerateRndSeedButtonListener implements OnClickListener {
        public void onClick(View v) {        	
           startActivity(new Intent(MainActivity.this, GetRandomSeedActivity.class));
       }        
    }
    
    private class ClearButtonListener implements OnClickListener {
        public void onClick(View v) { 
    		DialogFragment dialogFragment = MyAlertDialogFragment.newInstance(R.string.dialog_clear_alert_title, 
    				MyAlertDialogFragment.AlertDialogTypes.Clear);
        	dialogFragment.show(getFragmentManager(), "dialog");
       }        
    } 
	
	public static class MyAlertDialogFragment extends DialogFragment {

        public static MyAlertDialogFragment newInstance(int title, AlertDialogTypes alertDialogType) {
            MyAlertDialogFragment frag = new MyAlertDialogFragment();
            Bundle args = new Bundle();
            args.putInt("title", title);
            args.putInt("alertDialogType", alertDialogType.index());
            frag.setArguments(args);
            return frag;
        }

        @Override
        public Dialog onCreateDialog(Bundle savedInstanceState) {
            final int title = getArguments().getInt("title");
            final AlertDialogTypes alertDialogType = AlertDialogTypes.fromIndex(getArguments().getInt("alertDialogType"));

            return new AlertDialog.Builder(getActivity())
                .setIcon(R.drawable.emo_im_surprised)
                .setTitle(title)
                .setPositiveButton(R.string.alert_dialog_ok,
                    new DialogInterface.OnClickListener() {
                        public void onClick(DialogInterface dialog, int whichButton) {
                        	if (alertDialogType == AlertDialogTypes.Clear)
                        		((MainActivity)getActivity()).clearRandomSeed();
                        	else {
                        		((MainActivity)getActivity()).mStatusText.setText(R.string.status_init_text);
                        		RndGeneratorController.getInstance().initRndAlgorithm();
                        	}
                        }
                    }
                )
                .setNegativeButton(R.string.alert_dialog_cancel,
                    new DialogInterface.OnClickListener() {
                        public void onClick(DialogInterface dialog, int whichButton) {
                            
                        }
                    }
                )
                .create();
        }
        
        public enum AlertDialogTypes {
        	Clear(1),
        	Reinitialize(2);
        	
        	 private final int index;   

        	AlertDialogTypes(int index) {
        		this.index = index;
        	}

        	public int index() { 
        		return index; 
        	}
        	
        	public static AlertDialogTypes fromIndex(int index) {
        		for(AlertDialogTypes a:AlertDialogTypes.values()) {
        			if (a.index()==index) {
        				return a;
        			}
        		}        		
        		return null;
        	}
        }
    }
	
	private class GenerateButtonListener implements OnClickListener {
        public void onClick(View v) {
        	
        	int number_count = Integer.parseInt(mPrefs.getString("setting_number_count", "7"));
    		int grid_max_value = Integer.parseInt(mPrefs.getString("setting_grid_max_value", "35"));
        	
        	int[] selectedNumberArr = new int[number_count];
        	boolean[] usedNumbersArr = new boolean[grid_max_value+1]; 
        	int randomNumbersGenerated = 0;        	
        	final RndGeneratorController rndController = RndGeneratorController.getInstance();
        	
        	if (!rndController.isInitialized()) {
        		
        		if (rndController.getCurrentAlgorithm() == RndAlgorithm.Blum_Blum_Shub)
        			showUninitializedWarning();
        		else if (rndController.getCurrentAlgorithm() == RndAlgorithm.Random_ORG)
        			showReinitializeDialog();
        		
        		return;
        	}
        	
            int fairNumbers = Integer.MAX_VALUE / grid_max_value;
            int rndIntMax = fairNumbers * grid_max_value;
        	
        	while(randomNumbersGenerated < number_count) {        		
        		int rndInt;
        		try{
        			rndInt = rndController.getNextInt();
        		}
        		catch(UnInitializedException e) {
        			return;
        		}
        		if (rndInt < rndIntMax) {
        			rndInt = (rndInt % grid_max_value) + 1;
        			if (!usedNumbersArr[rndInt]) {
        				usedNumbersArr[rndInt] = true;
        				selectedNumberArr[randomNumbersGenerated++] = rndInt;
        			}    				
        		}
        	}
        	
        	mLottoView.setSelectedNumbers(selectedNumberArr);
       }        
    }
}