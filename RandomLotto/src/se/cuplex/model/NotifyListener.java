package se.cuplex.model;

public abstract interface NotifyListener {
	  public abstract void onNotify(NotificationTypes notificationType, String message);
	  public enum NotificationTypes {
		  RndInitVectorChanged,
		  RndInitialized,
		  RndOrgCallBegin,
		  RndOrgCallComplete,
		  RndOrgEmptyBuffer,
		  RndOrgBufferPositionChanged,
		  PreferencesChanged
	  };
}
