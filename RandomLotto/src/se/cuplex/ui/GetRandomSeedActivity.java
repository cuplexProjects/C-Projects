package se.cuplex.ui;

import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import se.cuplex.model.RndGeneratorController;
import android.app.ActionBar;
import android.app.Activity;
import android.app.FragmentManager;
import android.content.Intent;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.view.MotionEvent;
import android.widget.ProgressBar;
import android.widget.TextView;

public class GetRandomSeedActivity extends Activity {

	public GetRandomSeedActivity() {
		
	}
	
	private float mTouchX = 0;
	private float mTouchY = 0;
	private TextView mRndSequenceText;
	private MessageDigest mMessageDigest;
	private ProgressBar mControlPointPBar;
	private byte[] mCoordinateBuffer;
	private int mBufferPos;
	private boolean mIsComplete;
	
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_gen_rnd_seed);
        
        //Show action bar
        final ActionBar bar = getActionBar();
        bar.setDisplayOptions(ActionBar.DISPLAY_HOME_AS_UP | ActionBar.DISPLAY_SHOW_HOME);        
        bar.show();   
        
        mRndSequenceText = (TextView)findViewById(R.id.randomSeedLabel);
        mControlPointPBar = (ProgressBar)findViewById(R.id.controlPointPBar);
        
        if (savedInstanceState == null) {
        	mCoordinateBuffer = new byte[4000]; //500 x,y positions
            mBufferPos = 0;
            mIsComplete = false;
        }
        else {
            mCoordinateBuffer = savedInstanceState.getByteArray("CoordinateBuffer");
            mBufferPos = savedInstanceState.getInt("BufferPos");
            mIsComplete = savedInstanceState.getBoolean("IsComplete");
        }        
        
        mControlPointPBar.setMax(4000);        
        
        try {
			mMessageDigest = MessageDigest.getInstance("SHA-512");
		} catch (NoSuchAlgorithmException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
    }
    
    @Override
    public void onResume() {
    	super.onResume();
    }
    
    @Override
    public void onStop() {
    	super.onStop();
    	if (mBufferPos > 0 || mIsComplete) {   
	   		mMessageDigest.update(mCoordinateBuffer);
			byte[] mdbytes = mMessageDigest.digest();
			int controllPoints = mCoordinateBuffer.length;
			
			if (!mIsComplete)
				controllPoints = mBufferPos;
			
    		RndGeneratorController.getInstance().setRandomSeedAndNotify(mdbytes, controllPoints);
    	}
    }
    
    @Override
    public void onSaveInstanceState(Bundle savedInstanceState) {
      super.onSaveInstanceState(savedInstanceState);
      
      savedInstanceState.putByteArray("CoordinateBuffer", mCoordinateBuffer);
      savedInstanceState.putInt("BufferPos", mBufferPos);
      savedInstanceState.putBoolean("IsComplete", mIsComplete);      
    }
    
    @Override
    public void onRestoreInstanceState(Bundle savedInstanceState) {
      super.onRestoreInstanceState(savedInstanceState);
      
      mCoordinateBuffer = savedInstanceState.getByteArray("CoordinateBuffer");
      mBufferPos = savedInstanceState.getInt("BufferPos");
      mIsComplete = savedInstanceState.getBoolean("IsComplete");
      updateGUI();
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        getMenuInflater().inflate(R.menu.random_seed_activity, menu);
        return true;
    }
    
    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        switch (item.getItemId()) {
            case android.R.id.home:
                // app icon in action bar clicked; go home
                Intent intent = new Intent(this, MainActivity.class);
                intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                startActivity(intent);
                return true;
                
            case R.id.menu_about:
            		showAboutDialog();            		    		
            		return true;
    		case R.id.menu_settings:
            		startActivity(new Intent(GetRandomSeedActivity.this, SettingsActivity.class));
            		return true;
            default:
                return super.onOptionsItemSelected(item);
        }
    }
    
    private void showAboutDialog() {
        FragmentManager fragmentManager = getFragmentManager();
        AboutDialogFragment newFragment = new AboutDialogFragment();
        newFragment.show(fragmentManager, "about_dialog");
    }
    
    @Override
    public boolean onTouchEvent(MotionEvent event) {
        if (event.getAction() == MotionEvent.ACTION_MOVE) {
            mTouchX = event.getX();
            mTouchY = event.getY();
        } else {
            mTouchX = -1;
            mTouchY = -1;
        }
        
        //mRndSequenceText.setText("x=" + mTouchX + " y=" + mTouchY);
        updateHashValue();
        super.onTouchEvent(event);
        return true;
    }
    
    private void updateHashValue() {
    	if (mTouchX>=0 && mTouchY>=0 ) {
    		float[] floatInData = new float[2];
    		
    		floatInData[0]=mTouchX;
    		floatInData[1]=mTouchY;
    		
    		byte[] ftobArr = float2Byte(floatInData);    		
    		
    		for(int i=0; i<ftobArr.length;i++) {
    			mCoordinateBuffer[mBufferPos] = ftobArr[i];
    			mBufferPos++;
    			
    			if (mBufferPos >= mCoordinateBuffer.length) {
    				mBufferPos = 0;
    				mIsComplete= true;
    			}
			}    		
    		
    		updateGUI();
    	}    	
    }
    
    private void updateGUI() {
    	if (mMessageDigest != null) {
	   		mMessageDigest.update(mCoordinateBuffer);
			byte[] mdbytes = mMessageDigest.digest();
			
			//Convert to hex
			StringBuffer sb = new StringBuffer();
			for (int i = 0; i < mdbytes.length; i++) {
		          sb.append(Integer.toString((mdbytes[i] & 0xff) + 0x100, 16).substring(1));
	        }
			
			mRndSequenceText.setText(sb.toString());
			
			if (!mIsComplete)
				mControlPointPBar.setProgress(mBufferPos);
    	}
    }
    
  //float2Byte method - writes floats to byte array
    public static final byte[] float2Byte(float[] inData) {
      int j=0;
      int length=inData.length;
      byte[] outData=new byte[length*4];
      for (int i=0;i<length;i++) {
        int data=Float.floatToIntBits(inData[i]);
        outData[j++]=(byte)(data>>>24);
        outData[j++]=(byte)(data>>>16);
        outData[j++]=(byte)(data>>>8);
        outData[j++]=(byte)(data>>>0);
      }
      return outData;
    }
}