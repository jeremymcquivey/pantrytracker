export class StringHelper   {
    public static TrimExcess (value: string): string {
        let cleanedValue = value.trim();
        while(cleanedValue.indexOf('  ')!=-1) {
            cleanedValue = cleanedValue.replace('  ',' ');
        }

        return cleanedValue;
    }
}