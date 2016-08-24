package se.cuplex.model;

public class UnInitializedException extends Exception {	
	
	private String mMessage;
	private ExceptionReason mExceptionReason;
	
	public UnInitializedException() {
		
	}
	
	public UnInitializedException(ExceptionReason exceptionReason, String message) {
		mMessage = message;
		mExceptionReason = exceptionReason;
	}
	
	public String getMessage() {
		return mMessage;
	}
	
	public ExceptionReason getExceptionReason() {
		return mExceptionReason;
	}
	
	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	public static final String RandomOrgDataBufferEmptyMessage = "Random.org data empty, please try again"; 
	
	public enum ExceptionReason {
		RndBufferEmpty,
		NoInitVector
	}
}