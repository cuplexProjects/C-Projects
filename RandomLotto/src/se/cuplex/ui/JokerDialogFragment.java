package se.cuplex.ui;
import se.cuplex.model.NotifyListener;
import se.cuplex.model.RndGeneratorController;
import se.cuplex.model.UnInitializedException;
import android.app.Dialog;
import android.app.DialogFragment;
import android.app.FragmentManager;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.Window;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.TextView;

public class JokerDialogFragment extends DialogFragment implements NotifyListener {
    /** The system calls this to get the DialogFragment's layout, regardless
        of whether it's being displayed as a dialog or an embedded fragment. */
	private TextView mJokerText;
    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        // Inflate the layout to use as dialog or embedded fragment
        View v = inflater.inflate(R.layout.dialog_joker, container, false);
        
        //Bind button onClick
        Button closeButton = (Button)v.findViewById(R.id.close_button);   
        closeButton.setOnClickListener(new DialogDismissButtonListener());
        
        Button newButton = (Button)v.findViewById(R.id.new_button);   
        newButton.setOnClickListener(new DialogNewButtonListener());
        
        mJokerText = (TextView)v.findViewById(R.id.joker_text);
        
        //Set initial text or restore text
        if (savedInstanceState == null)
        	setJoker();
        else
        	mJokerText.setText(savedInstanceState.getString("JokerText"));
        
        RndGeneratorController.getInstance().addNotifyListener(this);
        return v;
    }    
    
    @Override
    public void onSaveInstanceState(Bundle savedInstanceState) {
      super.onSaveInstanceState(savedInstanceState);
      
      savedInstanceState.putString("JokerText", mJokerText.getText().toString());
    }
    
    @Override
    public void onStop() {
    	super.onStop();
    	RndGeneratorController.getInstance().removeNotifyListener(this);
    }
    
    private void setJoker() {
        try {
        	mJokerText.setText(RndGeneratorController.getInstance().getJoker());
        }
        catch (UnInitializedException e) {
        	if (e.getMessage() != null)
        		mJokerText.setText(e.getMessage());
		}  
    }
  
    /** The system calls this only when creating the layout in a dialog. */
    @Override
    public Dialog onCreateDialog(Bundle savedInstanceState) {
        // The only reason you might override this method when using onCreateView() is
        // to modify any dialog characteristics. For example, the dialog includes a
        // title by default, but your custom layout might not need it. So here you can
        // remove the dialog title, but you must call the superclass to get the Dialog.
        Dialog dialog = super.onCreateDialog(savedInstanceState);
        dialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
        
        return dialog;
    }
    
    private class DialogDismissButtonListener implements OnClickListener {
		 public void onClick(View v) {
			 //Find dialog
			 FragmentManager fragmentManager = getFragmentManager();
			 DialogFragment dialog = (DialogFragment)fragmentManager.findFragmentByTag("joker_dialog");
			 
			 if (dialog != null)
				 dialog.dismiss();
		 }
	}
    
    private class DialogNewButtonListener implements OnClickListener {
		public void onClick(View v) {
			setJoker();		   
		}
	}

	@Override
	public void onNotify(NotificationTypes notificationType, String message) {
		if (notificationType == NotificationTypes.RndInitialized && Boolean.valueOf(message))
			setJoker();
	}
}