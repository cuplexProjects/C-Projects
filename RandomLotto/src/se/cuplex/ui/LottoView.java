package se.cuplex.ui;

import android.content.Context;
import android.content.res.Resources;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Canvas;
import android.graphics.Paint;
import android.graphics.Bitmap.Config;
import android.graphics.BitmapFactory.Options;
import android.graphics.Paint.Style;
import android.graphics.drawable.Drawable;
import android.graphics.RectF;
import android.os.Bundle;
import android.os.Parcelable;
import android.util.AttributeSet;
import android.view.View;

public class LottoView extends View {
	
	private Paint mLinePaint;
	private Paint mFillPaint;
	private final float mMargin = 28f;
	private final int GRID_MIN_SIZE = 4;
	private final int GRID_MAX_SIZE = 9;	
	private int mGridSize = 6;
	private int mGridMaxValue = 35;	
	private int mNumberCount = 7;
    private final RectF mDstRect = new RectF();
    private final RectF mCellRect = new RectF();
    private final RectF mCellDrawRect = new RectF(); 
    private int[] mSelectedNumbers = new int[mNumberCount];
    private float mScaleX = 1;
    private float mScaleY = 1;
    
    private Bitmap mBmpCell;
    private Drawable mDrawableBg;
	
	public LottoView(Context context) {
		super(context);
		init();
	}

	public LottoView(Context context, AttributeSet attrs) {
		super(context, attrs);		
		init();
	}

	public LottoView(Context context, AttributeSet attrs, int defStyle) {
		super(context, attrs, defStyle);
		init();
	}
	
	@SuppressWarnings("deprecation")
	private void init() {		
		mLinePaint = new Paint();
        mLinePaint.setColor(0xFF808080);
        mLinePaint.setStrokeWidth(2);
        mLinePaint.setStyle(Style.STROKE);
        mLinePaint.setFlags(Paint.ANTI_ALIAS_FLAG);
    	
        
        mFillPaint = new Paint();
        mFillPaint.setColor(0xFFFFFFFF);
        mFillPaint.setStrokeWidth(5);
        mFillPaint.setStyle(Style.FILL);
        
        mBmpCell = getResBitmap(R.drawable.lib_cell);
        mDrawableBg = getResources().getDrawable(R.drawable.lib_bg);
        setBackgroundDrawable(mDrawableBg);
        //setBackground(mDrawableBg);
        
        if (isInEditMode()) {
        	int[] tmpSelectedNumbers = new int[20];        
        	tmpSelectedNumbers[0] = 1;
        	tmpSelectedNumbers[1] = 19;
        	tmpSelectedNumbers[2] = 30;
        	tmpSelectedNumbers[3] = 27;
        	tmpSelectedNumbers[4] = 35;
        	tmpSelectedNumbers[5] = 29;
        	tmpSelectedNumbers[6] = 49;
        	tmpSelectedNumbers[7] = 41;        	
        	tmpSelectedNumbers[8] = 70;
        	tmpSelectedNumbers[9] = 71;
        	
        	setSelectedNumbers(tmpSelectedNumbers);
        }        
	}
	
	public void setSelectedNumbers(int[] selectedNumberArr) {
		for (int i=0; i<mSelectedNumbers.length;i++) {
			if (selectedNumberArr.length>i) {
				if (selectedNumberArr[i]>0 && selectedNumberArr[i]<=mGridMaxValue)
					mSelectedNumbers[i] = selectedNumberArr[i];
			}
		}		
		
		this.invalidate();
	}
	
	public void setParameters(int gridSize, int maxValue, int numberCount) {
		mGridSize = gridSize;
		
		if (mGridSize < GRID_MIN_SIZE)
			mGridSize = GRID_MIN_SIZE;
		else if (mGridSize > GRID_MAX_SIZE)
			mGridSize = GRID_MAX_SIZE;
		
		mGridMaxValue = maxValue;
		if (mGridMaxValue>(mGridSize*mGridSize))
			mGridMaxValue = mGridSize*mGridSize;
		
		mNumberCount = numberCount;
		if (mNumberCount>(mGridSize*mGridSize))
			mNumberCount = mGridSize*mGridSize;
		
		if (mNumberCount>mGridMaxValue)
			mNumberCount = mGridMaxValue;	
		
		mSelectedNumbers = new int[mNumberCount];
		float lineStrokeWidth = mLinePaint.getStrokeWidth()*2f;
		mCellRect.set(0,0,(mDstRect.width()/(float)mGridSize)-lineStrokeWidth, (mDstRect.height()/(float)mGridSize)-lineStrokeWidth);
		this.invalidate();
	}	
	
	public void clearSelectedNumbers() {
		mSelectedNumbers = new int[mNumberCount];
		this.invalidate();
	}
	
    @Override
    protected void onSizeChanged(int w, int h, int oldw, int oldh) {
        super.onSizeChanged(w, h, oldw, oldh);
        //int size = w < h ? w : h;
        
        mScaleX = w/1000f;
        mScaleY = h/1000f;
        
        //MARGIN = size*(1f/35f); //25 pixels margin on each end at 1 Mega Pixel
        float lineStrokeWidth = mLinePaint.getStrokeWidth()*2;
        mDstRect.set(mMargin, mMargin, 1000 - mMargin, 1000 - mMargin);
        mCellRect.set(0,0,(mDstRect.width()/(float)mGridSize)-lineStrokeWidth, (mDstRect.height()/(float)mGridSize)-lineStrokeWidth);
    }
	
    @Override
    protected void onMeasure(int widthMeasureSpec, int heightMeasureSpec) {
        // Keep the view squared
        int w = MeasureSpec.getSize(widthMeasureSpec);
        int h = MeasureSpec.getSize(heightMeasureSpec);
        int d = w == 0 ? h : h == 0 ? w : w < h ? w : h;
        setMeasuredDimension(d, d);
    }
    
    @Override
    protected Parcelable onSaveInstanceState() {
        Bundle b = new Bundle();

        Parcelable s = super.onSaveInstanceState();
        b.putParcelable("gv_super_state", s);
        b.putBoolean("gv_en", isEnabled());
        b.putIntArray("SelectedNumbers", mSelectedNumbers);
        
        return b;
    }

    @Override
    protected void onRestoreInstanceState(Parcelable state) {

        if (!(state instanceof Bundle)) {
            // Not supposed to happen.
            super.onRestoreInstanceState(state);
            return;
        }

        Bundle b = (Bundle) state;
        Parcelable superState = b.getParcelable("gv_super_state");
        setEnabled(b.getBoolean("gv_en", true));
        mSelectedNumbers = b.getIntArray("SelectedNumbers");

        super.onRestoreInstanceState(superState);
    }
    
    @Override
    protected void onDraw(Canvas canvas) {
        super.onDraw(canvas);
               
        canvas.scale(mScaleX,mScaleY);
        
        //tmp
        /*
        mLinePaint.setStyle(Style.FILL);
        mLinePaint.setColor(0xFF000000);
        canvas.drawRect(mDstRect, mLinePaint);
        */

        //tmp end
        
        
        mLinePaint.setColor(0xFF808080);
        mLinePaint.setStyle(Style.STROKE);
        
        //draw lines
        float width = mDstRect.width();
        float step = (width / (float)mGridSize)+1f;
        
        for(int x=(int)(mDstRect.left+step-2f); x<width;x+=step){
        	canvas.drawLine(mDstRect.left, x, mDstRect.right, x, mLinePaint);
        	canvas.drawLine(x, mDstRect.top, x, mDstRect.bottom, mLinePaint);        	
        }
        
        //draw numbers
        float xStart = mMargin + mCellRect.width()/4f; 
        float x = xStart;
        float y = mMargin + (step/1.5f);
        float xSingleLetterMargin = step/8f;
        
        mLinePaint.setColor(0xFF202020);
        mLinePaint.setStyle(Style.FILL);
        mLinePaint.setTextSize(step/2);
        for(int i=1; i<=mGridMaxValue; i++){        	
        	
        	if (i<10)
        		canvas.drawText(Integer.toString(i), x + xSingleLetterMargin, y, mLinePaint);
        	else
        		canvas.drawText(Integer.toString(i), x, y, mLinePaint);
        	
        	x+=step;
        	
        	if (x>width) {
        		x=xStart;
        		y+=step;
        	}
        }
        
        float cellRectSize = mCellRect.width();
        float cellRectOffset = mDstRect.width()/(float)mGridSize;
        float leterCenter = cellRectSize/4f;
        mFillPaint.setColor(0xFF000000);
        mLinePaint.setColor(0xFFFFFFFF);
        float lineStrokeWidth = mLinePaint.getStrokeWidth()/(mGridSize/2f);
        
        for (int i=0; i<mSelectedNumbers.length; i++) {
        	if (mSelectedNumbers[i] > 0) {
        		x = (mSelectedNumbers[i]-1) % mGridSize; 
        		y = (mSelectedNumbers[i]-1) / mGridSize;        		
        		        		
    			x = (x * cellRectOffset) + mMargin + (x*lineStrokeWidth);
    			y = (y * cellRectOffset) + mMargin + (y*lineStrokeWidth);
        		
        		mCellDrawRect.set(x,y,x + cellRectSize, y + cellRectSize);        		
    			canvas.drawBitmap(mBmpCell,null,mCellDrawRect,mFillPaint);
        		
        		if (mSelectedNumbers[i] < 10)        		
        			canvas.drawText(Integer.toString(mSelectedNumbers[i]), x+leterCenter+xSingleLetterMargin, y + step/1.5f, mLinePaint);
        		else
        			canvas.drawText(Integer.toString(mSelectedNumbers[i]), x+leterCenter, y + step/1.5f, mLinePaint);
        	}        
        }
    }
    
    private Bitmap getResBitmap(int bmpResId) {
        Options opts = new Options();
        opts.inDither = false;

        Resources res = getResources();
        Bitmap bmp = BitmapFactory.decodeResource(res, bmpResId, opts);

        if (bmp == null && isInEditMode()) {
            // BitmapFactory.decodeResource doesn't work from the rendering
            // library in Eclipse's Graphical Layout Editor. Use this workaround instead.

            Drawable d = res.getDrawable(bmpResId);
            int w = d.getIntrinsicWidth();
            int h = d.getIntrinsicHeight();
            bmp = Bitmap.createBitmap(w, h, Config.ARGB_8888);
            Canvas c = new Canvas(bmp);
            d.setBounds(0, 0, w - 1, h - 1);
            d.draw(c);
        }

        return bmp;
    }
}