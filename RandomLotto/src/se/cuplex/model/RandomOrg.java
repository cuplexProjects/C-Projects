package se.cuplex.model;

import java.io.BufferedReader;
import java.io.InputStream;
import java.io.InputStreamReader;
import org.apache.http.HttpEntity;
import org.apache.http.HttpResponse;
import org.apache.http.client.HttpClient;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.impl.client.DefaultHttpClient;
import se.cuplex.model.NotifyListener.NotificationTypes;
import android.os.AsyncTask;
import android.util.Log;

public class RandomOrg implements RandomGenerator {
	public static final int RANDOM_BUFFER_SIZE = 3000;  //Number of bytes to fetch in each request
	private short[] mRndByteBuffer;
	private int mByteBufferPos;
	private HttpClient mHttpClient;
	private NotifyListener mNofityListener = null;	
	private boolean mIsInitialized = false;
	private boolean mIsUpdating = false;		
	
	public RandomOrg() {
		mHttpClient = new DefaultHttpClient();
		mRndByteBuffer = new short[RANDOM_BUFFER_SIZE];
		mByteBufferPos = 0;
	}
	
	public void initialize(NotifyListener nofityListener) {
		if (!mIsUpdating) {
			if (nofityListener != null)
				mNofityListener = nofityListener;

			mIsUpdating = true;			
			
			//Nofity
			if (mNofityListener != null)
				mNofityListener.onNotify(NotificationTypes.RndOrgCallBegin, "Fetching "+ (RANDOM_BUFFER_SIZE*8) + " random bits.");
			
			new UpdateHandler().execute();
		}
	}
	
	public void unRegisterNotifyListener() {
		mNofityListener = null;
	}
	
	public boolean isInitialized() {
		return mIsInitialized;
	}
	
	public int getByteBufferPos() {
		return mByteBufferPos;
	}
	
	private boolean updateRandomBuffer() {
	    HttpClient httpclient = mHttpClient;
    	
	    //20 chars per line and each char gives 62 combinations = 4.84375 byte per row
	    int numberOfStrings = (int)Math.ceil(((double)RANDOM_BUFFER_SIZE)/10d);
	    mByteBufferPos = 0;
	    
	    // Prepare a request object
	    HttpGet httpget = new HttpGet("http://www.random.org/strings/?num="+numberOfStrings+"&len=20&digits=on&upperalpha=on&loweralpha=on&unique=on&format=plain&rnd=new");	    
	    
	    // Execute the request
	    HttpResponse response;
	    try {
	        response = httpclient.execute(httpget);
	        // Examine the response status
	        Log.i("responce code",response.getStatusLine().toString());

	        // Get hold of the response entity
	        HttpEntity entity = response.getEntity();
	        // If the response does not enclose an entity, there is no need
	        // to worry about connection release

	        if (entity != null) {

	            InputStream instream = entity.getContent();
	            BufferedReader reader = new BufferedReader(new InputStreamReader(instream));
	            String lineData;
	            StringBuilder sb = new StringBuilder();
	            
	            while((lineData = reader.readLine()) != null) {
	            	sb.append(lineData);
	            }
	            instream.close();
	            
	            char[] sbArr = sb.toString().toCharArray();
	            int sbArrIndex = 0;
	            
	            if (sbArr.length < mRndByteBuffer.length*2)
	            	return false;
	            
	            //5 chars needed to fill every byte (4.84375 in theory but much harder to implement)
	            for (int i=0; i<mRndByteBuffer.length; i++) {
	            	
		            int a = ((getIntFrom62VariantChar(sbArr[sbArrIndex++])%31) & 0xf) << 4; //4 bits
		            int b = (getIntFrom62VariantChar(sbArr[sbArrIndex++])%31) & 0xf; //4 bits
	            		            	
	            	mRndByteBuffer[i] = (short)(a | b);
	            }	            
	            
	            mIsInitialized = true;
	            entity.consumeContent();
	            
	            //mRndByteBuffer[mByteBufferPos++]	            
	            //2 bits missing for every char
            	//62 possible values per char while one real char has 256 possible values = > 4 input chars per byte - 2 values/bits	            	            
	        }

	    } catch (Exception e) {
	    	Log.e("RandomLotto", e.getMessage(), e.getCause());
	    	return false;
	    }		
	    
		return mIsInitialized;
	}
	
	private int getIntFrom62VariantChar(char c) {
    	int n;	// 0-61
    	
    	//0-9 48-57
    	if (c>=48 && c<=57)
    		n = c - 48;
    	//A-Z 65-90
    	else if (c>=65 && c<=90)
    		n = c - 55;
    	// a-z 97-122
    	else
    		n = c - 61;
    	
    	return n;
	}
	
	@Override
	public int next(int numBits) throws UnInitializedException {
		if (!mIsInitialized)
			throw new UnInitializedException();
		else {
			//Max 32 bit => 4 byte
			int rndInt = 0;
			int bytes =  (int)Math.floor((double)numBits/(double)8.0);
			if (bytes > 4) {
				bytes = 4;
				numBits = 32;
			}
			
			//Verify that we don´t exceed the mRndByteBuffer    
			if (mByteBufferPos + bytes >= mRndByteBuffer.length) {
				this.mIsInitialized = false;
				
				if (mNofityListener != null)
					mNofityListener.onNotify(NotificationTypes.RndOrgEmptyBuffer, null);
				throw new UnInitializedException(UnInitializedException.ExceptionReason.RndBufferEmpty,"");
			}

			while (bytes > 0) {
				rndInt = rndInt | (mRndByteBuffer[mByteBufferPos++]<< ((bytes-1)*8));				
				bytes--;
			}
			
			//MSB
			if (numBits%8>0) {
				if (numBits>=8)
					rndInt = rndInt | (mRndByteBuffer[mByteBufferPos++]<< (numBits-8));
				else
					rndInt = mRndByteBuffer[mByteBufferPos++] & (0xff>>numBits);
			}
			
			return rndInt;
		}
	}
	

    protected void onUpdateRandomBufferComplete(Boolean result) {
    	if (mNofityListener != null) {    		
			mNofityListener.onNotify(NotificationTypes.RndInitialized, result.toString());
    		mNofityListener.onNotify(NotificationTypes.RndOrgCallComplete, result.toString());
    	}
    	mIsUpdating = false;
    	mIsInitialized = result;
    }
    
    private class UpdateHandler extends AsyncTask<Void,Void, Boolean> {
		@Override
		protected Boolean doInBackground(Void... params) {
			return updateRandomBuffer();
		}
		
		@Override
		protected void onPostExecute(Boolean result) {
			onUpdateRandomBufferComplete(result);
		}
    }
}
