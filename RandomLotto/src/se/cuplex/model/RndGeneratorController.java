package se.cuplex.model;

import java.math.BigInteger;
import java.security.SecureRandom;
import java.util.LinkedList;
import java.util.List;
import android.util.Log;

public class RndGeneratorController implements NotifyListener{
	private static final RndGeneratorController mInstance = new RndGeneratorController();;
	private byte[] mRndInitVector = null;
	private boolean mIsInitialized = false;
	private List<NotifyListener> mNotifyListeners = null;
	private RandomGenerator mRandomGenerator = null;
	private RndAlgorithm mAlgorithm = RndAlgorithm.None;
	
	private RndGeneratorController() {
		mNotifyListeners = new LinkedList<NotifyListener>();
	}
	
	//Input = 64 byte init vector
	public void setRandomSeedAndNotify(byte[] seed) {
		mRndInitVector = seed;
		notifyAll(NotificationTypes.RndInitVectorChanged, null);
	}
	//Input = 64 byte init vector
	public void setRandomSeedAndNotify(byte[] seed, int controlPoints) {
		mRndInitVector = seed;
		if (this.getCurrentAlgorithm() == RndAlgorithm.Blum_Blum_Shub)
			notifyAll(NotificationTypes.RndInitVectorChanged, "using " + controlPoints + " control points");
	}
	
	public byte[] getRandomSeed() {
		return mRndInitVector;
	}
	
	public void addNotifyListener(NotifyListener listener) {		
		if (!mNotifyListeners.contains(listener))
			mNotifyListeners.add(listener);
	}
	
	public boolean removeNotifyListener(NotifyListener listener) {
		return mNotifyListeners.remove(listener);
	}
	
	public void clearDataStructures() {
		mRandomGenerator = null;
		mRndInitVector = null;
		mIsInitialized = false;
	}
	
	public boolean isInitialized() {
		return mIsInitialized;
	}
	
	public RndAlgorithm getCurrentAlgorithm() {
		return mAlgorithm;
	}
	
	public void setRandomAlgorithm(RndAlgorithm a) {
		mRndInitVector = null;
		mIsInitialized = false;
		mAlgorithm = a;
		initRndAlgorithm();
	}
	
	public int getNextInt() throws UnInitializedException {
		if (mRandomGenerator == null)
			return 0;
		else {
			int i = mRandomGenerator.next(31);  //31 bit = always positive integer
			if (mRandomGenerator instanceof RandomOrg)
				notifyAll(NotificationTypes.RndOrgBufferPositionChanged, Integer.toString(((RandomOrg)mRandomGenerator).getByteBufferPos()));
			return i;
		}			
	}	
	
	public String getJoker() throws UnInitializedException {
		if (mRandomGenerator == null)
			return "";
		else
		{ 
			String joker = "";
			
			while (joker.length()<7) {			
				int a = mRandomGenerator.next(4);  //integer between 0-15
				if (a<10 && a>0)
					joker+= Integer.toString(a);
			}
			
			if (mRandomGenerator instanceof RandomOrg)
				notifyAll(NotificationTypes.RndOrgBufferPositionChanged, Integer.toString(((RandomOrg)mRandomGenerator).getByteBufferPos()));
			
			return joker;
		}
	}
	
	private void notifyAll(NotificationTypes notificationType, String message) {
		for (int i=0; i<mNotifyListeners.size(); i++) {
			mNotifyListeners.get(i).onNotify(notificationType, message);
		}
	}
	
	public void preferencesChanged(String key) {
		notifyAll(NotificationTypes.PreferencesChanged, key);
	}
	
	public boolean initRndAlgorithm() {
		
		//Take care of previous instance
		if (mRandomGenerator != null && mRandomGenerator instanceof RandomOrg) {			
			((RandomOrg)mRandomGenerator).unRegisterNotifyListener();

			mRandomGenerator = null;
		}
		
		if (mAlgorithm == RndAlgorithm.Blum_Blum_Shub) {
			if (mRndInitVector == null)
				return false;
			
			int bitsize = 512;
			BigInteger nval = BlumBlumShub.generateN(bitsize, new SecureRandom());			
	
			// now create an instance of BlumBlumShub
			BlumBlumShub bbs = new BlumBlumShub(nval, mRndInitVector);
			mRandomGenerator = bbs;
			
			mIsInitialized = true;
			return true;
		}
		else if (mAlgorithm == RndAlgorithm.Blum_Blum_Shub_Auto_Seed) {
			SecureRandom r = new SecureRandom();
			int bitsize = 512;
			BigInteger nval = BlumBlumShub.generateN(bitsize, r);
			
			byte[] seed = new byte[bitsize/8];
			r.nextBytes(seed);
			
			// now create an instance of BlumBlumShub
			BlumBlumShub bbs = new BlumBlumShub(nval, seed);
			mRandomGenerator = bbs;
			
			mIsInitialized = true;
			this.notifyAll(NotificationTypes.RndInitialized, "true");
			return true;			
		}
		else if (mAlgorithm == RndAlgorithm.Random_ORG){
			RandomOrg randOrg = new RandomOrg();
			randOrg.initialize(this);
			mRandomGenerator = randOrg;
			mIsInitialized = false;
			
			return mIsInitialized;
		}
		
		return false;
	}
	
	///Static methods
	public static RndGeneratorController getInstance() {
		return mInstance;
	}
	
	public enum RndAlgorithm {
		None(0),
		Blum_Blum_Shub(1),
		Blum_Blum_Shub_Auto_Seed(2),
		Random_ORG(3);
		
		private final int index;   

		RndAlgorithm(int index) {
    		this.index = index;
    	}

    	public int index() { 
    		return index; 
    	}
    	
    	public static RndAlgorithm fromIndex(int index) {
    		for(RndAlgorithm a:RndAlgorithm.values()) {
    			if (a.index()==index) {
    				return a;
    			}
    		}        		
    		return null;
    	}		
	}
	
	public static class RndAlgorithmHelper {
		public static String GetRndAlgorithmName(RndAlgorithm a) {
			switch(a) {
				case Blum_Blum_Shub:
					return "Pseudo random generator (BBS)";
				case Blum_Blum_Shub_Auto_Seed:
					return "Pseudo random generator with automatic seeding (BBS)";
				case Random_ORG:
					return "Random.Org";
				default: 
					return null;
			}
		}
		public static RndAlgorithm GetRndAlgorithmFromValue(String val) {			
			int index = -1;
			try {
				index = Integer.parseInt(val);
				return RndAlgorithm.fromIndex(index);
			}
			catch(Exception e) {
				Log.e("Lotto", e.getMessage());
			}
			return null;
		}
	}

	@Override
	public void onNotify(NotificationTypes notificationType, String message) {
		if (notificationType == NotificationTypes.RndInitialized) {
			if (Boolean.valueOf(message)) {
				mIsInitialized = true;
			}
			
			notifyAll(notificationType, message);
		}
		
		else if (notificationType == NotificationTypes.RndOrgEmptyBuffer) {
			mIsInitialized = false;
			notifyAll(notificationType, message);
		}		
	}
}
