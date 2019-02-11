package se.cuplex.ui;
import android.app.Dialog;
import android.app.DialogFragment;
import android.app.FragmentManager;
import android.content.Intent;
import android.content.pm.PackageInfo;
import android.content.pm.PackageManager.NameNotFoundException;
import android.net.Uri;
import android.os.Bundle;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.Window;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.TextView;

public class AboutDialogFragment extends DialogFragment {
    /** The system calls this to get the DialogFragment's layout, regardless
        of whether it's being displayed as a dialog or an embedded fragment. */
    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
            Bundle savedInstanceState) {
        // Inflate the layout to use as dialog or embedded fragment
        View v = inflater.inflate(R.layout.dialog_about, container, false);
        
        //Bind button onClick
        Button closeButton = (Button)v.findViewById(R.id.close_button);   
        closeButton.setOnClickListener(new DialogDismissButtonListener());
        
        Button rateButton = (Button)v.findViewById(R.id.rate_button);   
        rateButton.setOnClickListener(new DialogRateButtonListener());
        
        //Set version text        
        PackageInfo pInfo;
        String version = "";
		try {
			pInfo = v.getContext().getPackageManager().getPackageInfo(v.getContext().getPackageName(), 0);
			version = " " + pInfo.versionName;
		} catch (NameNotFoundException e) {			
			e.printStackTrace();
		}
		
        TextView versionText = (TextView)v.findViewById(R.id.about_version);
        versionText.setText(getResources().getString(R.string.main_version_text) + version);        
        return v;
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
			 DialogFragment dialog = (DialogFragment)fragmentManager.findFragmentByTag("about_dialog");
			 
			 if (dialog != null)
				 dialog.dismiss();
		 }
	}
    
    private class DialogRateButtonListener implements OnClickListener {
		 public void onClick(View v) {
			 //Find dialog
			 FragmentManager fragmentManager = getFragmentManager();
			 DialogFragment dialog = (DialogFragment)fragmentManager.findFragmentByTag("about_dialog"); 				 
			 
			 try{
				 Intent intent = new Intent(Intent.ACTION_VIEW);
				 intent.setData(Uri.parse("market://details?id=se.cuplex.ui"));
				 startActivity(intent);
			 }
			 catch (Exception e) {
				 Log.println(1,"Exception",e.getMessage());				
			}
			 
			 if (dialog != null)
				 dialog.dismiss();
		 }
	}
}
