export class DisplayTransactionModel {
  public id: string;
  public amount: number;
  public storedAmount: number;
  public isRepeatable: boolean;
  public transactionType: string;
  public transactionCategory: string;
  public transactionDate: Date;
}
