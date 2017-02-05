package md5177888fa8374e03981c7b7648afccec0;


public class SatelliteMenuButton_SavedState
	extends android.view.View.BaseSavedState
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_describeContents:()I:GetDescribeContentsHandler\n" +
			"n_writeToParcel:(Landroid/os/Parcel;I)V:GetWriteToParcel_Landroid_os_Parcel_IHandler\n" +
			"";
		mono.android.Runtime.register ("bottomtabbedpage.Droid.Satellite.SatelliteMenuButton+SavedState, bottomtabbedpage.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", SatelliteMenuButton_SavedState.class, __md_methods);
	}


	public SatelliteMenuButton_SavedState (android.os.Parcel p0) throws java.lang.Throwable
	{
		super (p0);
		if (getClass () == SatelliteMenuButton_SavedState.class)
			mono.android.TypeManager.Activate ("bottomtabbedpage.Droid.Satellite.SatelliteMenuButton+SavedState, bottomtabbedpage.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.OS.Parcel, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}


	public SatelliteMenuButton_SavedState (android.os.Parcelable p0) throws java.lang.Throwable
	{
		super (p0);
		if (getClass () == SatelliteMenuButton_SavedState.class)
			mono.android.TypeManager.Activate ("bottomtabbedpage.Droid.Satellite.SatelliteMenuButton+SavedState, bottomtabbedpage.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.OS.IParcelable, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}


	public int describeContents ()
	{
		return n_describeContents ();
	}

	private native int n_describeContents ();


	public void writeToParcel (android.os.Parcel p0, int p1)
	{
		n_writeToParcel (p0, p1);
	}

	private native void n_writeToParcel (android.os.Parcel p0, int p1);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
