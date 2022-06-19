# Wizard Example

Trying to get a wizard-style control to work in Avalonia

[WizardStyles.axaml](WizardExample/Controls/WizardStyles.axaml) has lots of comments about previous things I've tried.

Comments in [WizardControl.cs](WizardExample/Controls/WizardControl.cs) are mostly features/enhancement ideas.

[MainWindow.axaml](WizardExample/Views/MainWindow.axaml) is a simple usage example.
- Page 1 should have the back button disabled (BackEnabled)
- Page 2 shouldn't enable the entered text length is greater than 5 (NextEnabled)
- Page 3 should have the "Next" button say "Finish" (NextContent)