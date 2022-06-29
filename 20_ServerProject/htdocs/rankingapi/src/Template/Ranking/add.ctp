<?php
/**
 * @var \App\View\AppView $this
 * @var \App\Model\Entity\Ranking $ranking
 */
?>
<nav class="large-3 medium-4 columns" id="actions-sidebar">
    <ul class="side-nav">
        <li class="heading"><?= __('Actions') ?></li>
        <li><?= $this->Html->link(__('List Ranking'), ['action' => 'index']) ?></li>
    </ul>
</nav>
<div class="ranking form large-9 medium-8 columns content">
    <?= $this->Form->create($ranking) ?>
    <fieldset>
        <legend><?= __('Add Ranking') ?></legend>
        <?php
            echo $this->Form->control('Name');
            echo $this->Form->control('Time');
            echo $this->Form->control('Date');
        ?>
    </fieldset>
    <?= $this->Form->button(__('Submit')) ?>
    <?= $this->Form->end() ?>
</div>
