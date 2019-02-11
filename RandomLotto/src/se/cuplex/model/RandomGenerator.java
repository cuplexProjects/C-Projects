package se.cuplex.model;

public interface RandomGenerator {
	   public int next(int numBits) throws UnInitializedException;
} 